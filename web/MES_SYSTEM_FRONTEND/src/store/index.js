import Vue from 'vue'
import Vuex from 'vuex'
import * as mutations from './mutations'
import * as actions from './actions'
import * as getters from './getters'

Vue.use(Vuex);

const state = {
  token: '94CA11E813946F5E475BA2E6E05FD5B0',
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
