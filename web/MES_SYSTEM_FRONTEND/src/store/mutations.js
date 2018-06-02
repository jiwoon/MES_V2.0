export const setLoginToken = (state, token) => {
  state.token = token;
};
export const setTableRouter = (state, tableRouterData) => {
  state.tableRouterApi = tableRouterData;
};

export const setLoading = (state, isLoading) => {
  state.isLoading = isLoading;
};

export const setRouter = (state, routerIn) => {
  state.routerIn = routerIn
};
export const setEditing = (state, isEditing) => {
  state.isEditing = isEditing
};
export const setEditData = (state, editData) => {
  state.editData = editData
};
export const setCopyData = (state, copyData) => {
  state.copyData = copyData
};
