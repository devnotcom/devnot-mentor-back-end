using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace DevnotMentor.Api.Services
{
    public class MentorService : BaseService, IMentorService
    {
        MentorRepository repository;
        MentorLinksRepository mentorLinksRepository;
        MentorTagsRepository mentorTagsRepository;
        TagRepository tagRepository;
        UserRepository userRepository;

        public MentorService(IOptions<AppSettings> appSettings, IOptions<ResponseMessages> responseMessages, IMapper mapper, MentorDBContext context) : base(appSettings, responseMessages, mapper, context)
        {
            this.mapper = mapper;
            repository = new MentorRepository(context);
            mentorLinksRepository = new MentorLinksRepository(context);
            mentorTagsRepository = new MentorTagsRepository(context);
            tagRepository = new TagRepository(context);
            userRepository = new UserRepository(context);
        }

        public async Task<ApiResponse<MentorProfileModel>> GetMentorProfile(string userName)
        {
            var response = new ApiResponse<MentorProfileModel>();

            await RunInTry(response, async () => {
                var user = userRepository.Filter(u => u.UserName == userName).FirstOrDefault();

                if(user != null)
                {
                    var mentor = repository.Filter(m => m.UserId == user.Id).FirstOrDefault();

                    if(mentor != null)
                    {
                        response.Data = mapper.Map<MentorProfileModel>(mentor);
                        response.Success = true;
                    }
                    else
                    {
                        response.Message = responseMessages.Values["MentorNotFound"];
                    }
                }
                else
                {
                    response.Message = responseMessages.Values["UserNotFound"];
                }
            });

            return response;
        }

        public async Task<ApiResponse<MentorProfileModel>> CreateMentorProfile(MentorProfileModel model)
        {
            var response = new ApiResponse<MentorProfileModel>();

            await RunInTry(response, async () => {
                
                var user = userRepository.Filter(u => u.UserName == model.UserName).FirstOrDefault();
                
                if(user != null)
                { 
                    var isRegisteredMentor = repository.Filter(m => m.UserId == user.Id).Any();

                    if (!isRegisteredMentor)
                    {
                        var mentor = await CreateNewMentor(model, user);

                        if (mentor != null)
                        {
                            response.Data = mapper.Map<MentorProfileModel>(mentor);
                            response.Success = true;
                        }
                        else
                        {
                            response.Message = responseMessages.Values["UnhandledException"];
                        }
                    }
                    else
                    {
                        response.Message = responseMessages.Values["AlreadyRegisteredMentor"];
                    }
                }
                else
                {
                    response.Message = responseMessages.Values["UserNotFound"];
                }

            });

            return response;
        }

        public Task<List<MentorProfileModel>> SearchMentor(SearchMentorModel model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMentorProfile(MentorProfileModel model)
        {
            throw new NotImplementedException();
        }


        private async Task<Mentor> CreateNewMentor(MentorProfileModel model, User user)
        {
            Mentor mentor = null;

            using (var transaction = new TransactionScope())
            {
                var newMentor = mapper.Map<Mentor>(model);
                newMentor.UserId = user.Id;

                mentor = repository.Create(newMentor);
                if (mentor != null)
                {
                    mentorLinksRepository.Create(mentor.Id, model.MentorLinks);

                    foreach (var mentorTag in model.MentorTags)
                    {
                        var tag = tagRepository.Filter(t => t.Name == mentorTag).FirstOrDefault();
                        if (tag != null)
                        {
                            mentorTagsRepository.Create(new MentorTags { TagId = tag.Id, MentorId = mentor.Id });
                        }
                        else
                        {
                            var newTag = tagRepository.Create(new Tag { Name = mentorTag });
                            if (newTag != null)
                            {
                                mentorTagsRepository.Create(new MentorTags { TagId = newTag.Id, MentorId = mentor.Id });
                            }
                        }
                    }
                    await repository.SaveChangesAsync();
                    transaction.Complete();
                }
            }

            return mentor;
        }
    }
}
