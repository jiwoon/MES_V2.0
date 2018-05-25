import Vue from 'vue'
import Vuex from 'vuex'
import * as mutations from './mutations'
import * as actions from './actions'
import * as getters from './getters'

Vue.use(Vuex);

const state = {
  isLoading: false,
  routerApi: 'default'
};

const store = new Vuex.Store({
  state,
  getters,
  actions,
  mutations
});

export default store;
