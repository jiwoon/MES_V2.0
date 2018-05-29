export const setTableRouter = ({commit}, tableRouterData) => {
  commit('setTableRouter', tableRouterData)
};

export const setLoading = ({commit}, isLoading) => {
  commit('setLoading', isLoading)
};

export const setRouter = ({commit}, routerIn) => {
  commit('setRouter', routerIn)
};
