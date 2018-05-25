import Vue from 'vue'
import Router from 'vue-router'
import Main from '../pages/main/Main'
import Login from '../pages/login/Login'

import Details from '../pages/main/details/Details'
Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      redirect: '/main'
    },
    {
      path: '/main',
      name: 'Main',
      component: Main,
      children: [
        {
          path: 'details',
          component: Details
        }
      ]
    },
    {
      path: '/login',
      name: 'Login',
      component: Login
    }
  ]
})
