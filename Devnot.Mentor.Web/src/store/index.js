import Vue from 'vue';
import Vuex from 'vuex';
import landingPage from '../modules/landingPage/store';
import login from '../modules/login/store';


Vue.use(Vuex);

export const storeOptions = {
    modules: {
    landingPage,
    login,

  },
};

export default new Vuex.Store(storeOptions);
