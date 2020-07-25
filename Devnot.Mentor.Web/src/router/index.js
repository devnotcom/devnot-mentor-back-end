import Vue from 'vue'
import VueRouter from 'vue-router'
import Login from '../modules/login';
import LandingPage from '../modules/landingPage'
import Register from '../modules/register';
import Hello from '../modules/HelloWorld';

Vue.use(VueRouter)

  const routes = [
  {
    path: '/landing', // Varsayılan giriş daha sonradan burası olacaktır.
    name: 'LandingPage',
    component: LandingPage,
    meta: {hasLayout: true},
    
  },
  {
    path: '/',
    name: 'Hello',
    component: Hello,
    meta: {hasLayout: true},
    
  },  
  {
    path: '/Login',
    name: 'Login',
    component: Login,
    meta: {hasLayout: false},
  },
  {
    path: '/Register',
    name: 'Register',
    component: Register,
    meta: {hasLayout: false},
  }, 
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
