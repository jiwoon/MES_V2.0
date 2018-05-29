export const setTableRouter = (state, tableRouterData) => {
  state.tableRouterApi = tableRouterData;
};

export const setLoading = (state, isLoading) => {
  state.isLoading = isLoading;
};

export const setRouter = (state, routerIn) => {
  state.routerIn = routerIn
};
