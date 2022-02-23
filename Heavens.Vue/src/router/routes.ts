import { cloneDeep } from 'lodash-es'
import userLayout from 'src/layouts/user-layout.vue'
import { staticRoles } from 'src/store/user/state'
import { MenuDataItem } from './_typing'

// 动态路由，根据用户信息中的角色
export const dynamicRouter: MenuDataItem[] = [
  {
    path: '/home',
    name: 'home',
    meta: { title: '主页', icon: 'r_home', authority: [staticRoles.admin, staticRoles.user] },
    component: import('pages/home/index.vue')
  },
  {
    path: '/user',
    name: 'user',
    meta: { title: '用户管理', icon: 'r_manage_accounts', authority: [staticRoles.admin] },
    component: import('pages/user/index.vue')
  }
]

for (let index = 1; index <= 14; index++) {
  const element = cloneDeep(dynamicRouter[0])
  element.path = '/home' + index
  element.name = 'home' + index
  element.component = import(`pages/homeTest/index${index}.vue`)
  {
    ;(element.meta as any).title =
      (element.meta as any).title + (element.meta as any).title + (element.meta as any).title + index
  }
  dynamicRouter.push(element)
}

// 静态路由
export const staticRoutes: MenuDataItem[] = [
  {
    path: '/login',
    name: 'Login',
    redirect: '/login',
    component: userLayout,
    children: [
      {
        path: '/login',
        name: 'login',
        meta: { title: '登录' },
        component: import('pages/user/login.vue')
      },
      {
        path: '/register',
        name: 'register',
        meta: { title: '注册' },
        component: import('pages/user/register.vue')
      }
    ]
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: import('pages/Error404.vue')
  }
]
