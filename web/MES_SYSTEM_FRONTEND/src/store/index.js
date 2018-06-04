import Vue from 'vue'
import Vuex from 'vuex'
import * as mutations from './mutations'
import * as actions from './actions'
import * as getters from './getters'

Vue.use(Vuex);

const state = {
  token: '',
  isLoading: false,
  tableRouterApi: 'default',
  routerIn: 'table',
  isEditing: false,
  editData: [],
  copyData: []
};

const store = new Vuex.Store({
  state,
  getters,
  actions,
  mutations
});

export default store;
