<script>
import LandingPage from './modules/landingPage/index';

export default {
  name: 'App',
  components: {
    LandingPage,
  },
  props: {
    source: String,
  },

  data: () => ({
    isLogin: false,
  }),
  computed: {
    hasLayout() {
      return this.$route.meta.hasLayout;
    },

  },
  created () {
    this.$vuetify.theme.dark = false
  },
  methods: {
    setEmitLogin() {
      console.log(this.isLogin);
      
      this.isLogin = true;
      console.log(this.isLogin);

    }
  }
};
</script>

<template>
  <v-app>
    <template v-if="!hasLayout">
       <router-view  />
    </template>
    <landing-page v-if="hasLayout === true"/>
  </v-app>
  

</template>

<style lang="scss">
.v-application a {
    color: #1976d2;
}

a:-webkit-any-link {
  text-decoration: none
}
</style>