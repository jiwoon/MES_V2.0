/*vue-router 配置*/

import Vue from 'vue'
import Router from 'vue-router'
import store from '../store'
import Main from '../pages/Main'
import TableMain from '../pages/table/TableMain'
import Login from '../pages/login/Login'
import test from '../components/test'
import TableModule from '../pages/table/details/TableModule'
import SettingMain from '../pages/setting/SettingMain'
import OrderManage from '../pages/setting/details/OrderManage'

Vue.use(Router);

const router = new Router({
  routes: [
    {
      path: '/_empty'
    },

    {
      path: '/',
      component: Main,
      redirect: '/table',
      meta: {
        requireAuth: true
      },
      children: [
        {
          path: '/table',
          name: 'Table',
          component: TableMain,
          children: [
            {
              path: 'details',
              component: TableModule
            }
          ]
        },
        {
          path: '/setting',
          name: 'Setting',
          component: SettingMain,
          children: [
            {
              path: 'order_manage',
              component: OrderManage
            }
          ]
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

if (localStorage.getItem('token')) {
  store.commit('setLoginToken', localStorage.getItem('token'))
}


router.beforeEach((to, from, next) => {
  if (to.matched.some(r => r.meta.requireAuth)) {
    console.log("1")
    if (store.state.token) {
      next();
    } else {
      next({
        path: '/login',
        query: {redirect: to.fullPath}
      })
    }
  } else {
    next();
  }
});

export default router;
