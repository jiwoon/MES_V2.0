export const setRouter = ({commit}, routerData) => {
  commit('setRouter', routerData)
};

export const setLoading = ({commit}, isLoading) => {
  commit('setLoading', isLoading)
};
