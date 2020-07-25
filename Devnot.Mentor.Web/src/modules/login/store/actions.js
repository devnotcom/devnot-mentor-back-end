import axios from 'axios';

export default{
  login({ commit }, modelData) {
    console.log("modelData: " + modelData);
    axios
    .post('http://localhost:59166/user/login/', {
      userName: modelData.username,
      password: modelData.password
    }
  )
    .then(response => {

      if(response.data.token) {
        console.log(response.data.token);
        commit('setIsLoggedIn');
      }
      else {
        console.log('işlem başarısız');
        alert('Hatalı kullanıcı adı veya parola');
      }
    })
    .catch(error => console.log(error.message))
    
  }
}