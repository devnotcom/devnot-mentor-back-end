<script>
import { mapState } from 'vuex';
export default {
 name : 'langingPage',
 props: {
    source: String,
  },

  data: () => ({
    drawer: false,
    isLogin: true,
    isDarkMode: false,
  }),
  watch: {
    isDarkMode() {
       this.$vuetify.theme.dark = this.isDarkMode;
    },
  },
  computed: {
    ...mapState('login', ['isLoggedIn']),
  },
  created () {
    this.$vuetify.theme.dark = this.isDarkMode;
  },
  methods: {
    emitLogin() {
      this.isLogin = false;
     this.$router.push('/Login');
    }
  },


}
</script>

<template>
<div>
    <v-navigation-drawer
      v-model="drawer"
      app
      clipped
    >
      <v-list dense>
        <v-list-item link>
          <v-list-item-action>
            <v-icon>mdi-view-dashboard</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Dashboard</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item link>
          <v-list-item-action>
            <v-icon>mdi-settings</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Settings</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar
      app
      clipped-left
    >
      <v-app-bar-nav-icon v-if="isLoggedIn" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-toolbar-title>Devnot</v-toolbar-title>
      <v-spacer></v-spacer>
     <!-- <v-toolbar-title><v-btn text small @click="router('login')">Login</v-btn></v-toolbar-title>-->
     <v-toolbar-title style="margin-right:20px">Is Login: {{isLoggedIn}}</v-toolbar-title>
     <v-toolbar-title v-if="!isLoggedIn"><v-btn text small><router-link to="/Login">Login</router-link></v-btn></v-toolbar-title>
     <v-toolbar-title v-if="!isLoggedIn"><v-btn text small><router-link to="/Register">Register</router-link></v-btn></v-toolbar-title>
    </v-app-bar>

    <v-content>
      <router-view></router-view>
    </v-content>
      <v-row justify="center">
        <v-switch
          v-model="isDarkMode"
          label="Dark Mode"
        ></v-switch>
        {{isDarkMode}}
      </v-row>
    <v-footer app>
      <span>&copy;Devnot 2020</span>
    </v-footer>
      <!-- <v-container
        class="fill-height"
        fluid
      >
        <v-row
          align="center"
          justify="center"
        >
          <v-col class="shrink">
            <v-tooltip right>
              <template v-slot:activator="{ on }">
                <v-btn
                  :href="source"
                  icon
                  large
                  target="_blank"
                  v-on="on"
                >
                  <v-icon large>mdi-code-tags</v-icon>
                </v-btn>
              </template>
              <span>LANDING PAGE</span>
            </v-tooltip>
          </v-col>
        </v-row>
      </v-container> -->
      </div>
</template>

<style>

</style>