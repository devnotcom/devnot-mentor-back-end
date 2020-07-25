<script>
import { mapActions} from 'vuex';

  export default {
    props: {
      source: String,
    },
    data() {
      return {
        formData: {},
      }
    },
    methods: {
      ...mapActions('login', ['login']),
      ...mapActions('landingPage', ['test']),
      logMe() {
        this.login(this.formData);
        this.$router.push('/');
      }
    }
  }
</script>

<template>
      <v-container
        class="fill-height"
        fluid
      >
        <v-row
          align="center"
          justify="center"
        >
          <v-col
            cols="12"
            sm="6"
            md="6"
            lg="5"
          >
            <v-card class="elevation-12">
              <v-toolbar
                color="primary"
                dark
                flat
              >
                <v-toolbar-title>Login form</v-toolbar-title>
                <v-spacer></v-spacer>
              </v-toolbar>
              <v-card-text>
                <v-form>
                  <v-text-field
                    v-model="formData.username"
                    label="Login"
                    name="login"
                    type="text"
                  ></v-text-field>

                  <v-text-field
                    v-model="formData.password"
                    id="password"
                    label="Password"
                    name="password"
                    type="password"
                  ></v-text-field>
                  {{formData}}
                </v-form>
              </v-card-text>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn @click="logMe" color="primary">Login</v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
</template>
