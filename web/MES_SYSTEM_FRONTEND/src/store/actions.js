export const setLoginToken = ({commit}, token) => {
  commit('setLoginToken', token)
};

export const setTableRouter = ({commit}, tableRouterData) => {
  commit('setTableRouter', tableRouterData)
};

export const setLoading = ({commit}, isLoading) => {
  commit('setLoading', isLoading)
};

export const setRouter = ({commit}, routerIn) => {
  commit('setRouter', routerIn)
};
export const setEditing = ({commit}, isEditing) => {
  commit('setEditing', isEditing)
};
export const setEditData = ({commit}, editData) => {
  commit('setEditData', editData)
};
export const setCopyData = ({commit}, copyData) => {
  commit('setCopyData', copyData)
};
