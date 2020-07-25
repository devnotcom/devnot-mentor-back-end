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
                <v-toolbar-title>Register form</v-toolbar-title>
                <v-spacer></v-spacer>
              </v-toolbar>
              <v-card-text>
                <v-form>
                  <v-text-field
                    v-model="label"
                    label="Kullanıcı Adı"
                  ></v-text-field>

                  <v-text-field
                    v-model="label"
                    label="E Mail"
                  ></v-text-field>                  

                  <v-text-field
                    v-model="password"
                    :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
                    :rules="[rules.required, rules.min]"
                    :type="show1 ? 'text' : 'password'"
                    name="input-10-1"
                    label="Şifre"
                    hint="8 Karakter Girildi.."
                    counter
                    @click:append="show1 = !show1"
                  ></v-text-field>
                </v-form>
              </v-card-text>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="primary">Register</v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
</template>

<script>
export default {
  data() {
    return {
      show1: false,
      rules: {
        required: value => !!value || 'Bu alanın doldurulması zorunludur.',
        min: v => v.length >= 8 || 'En az 8 karakter girilmelidir.',
        emailMatch: () => ('The email and password you entered don\'t match'),
      },
    }
  }
}
</script>

<style>

</style>