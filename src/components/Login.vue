<script setup lang="ts">
import axios from 'axios';
import {ref} from "vue";
import {useRouter} from 'vue-router';

const router = useRouter();
const API_URL = "http://localhost:5208/";
const errorMessage = ref<string>();
type Credential = {
  email: string;
  password: string;
}
const credentials = ref<Credential>({} as Credential);
const login = async () => {
  //const router = useRouter();//--
  try {
    const response = await axios.post(`${API_URL}api/FirstApp/Login`, credentials.value);
    const { role } = response.data;
    // const role = 1;
    console.log("role", role);

    if (role === 1) {
      console.log("router", router);
      router.push('/admin');

    } else if (role === 2) {
      router.push('/user');
    } else {
      errorMessage.value = 'Invalid role.';
    }
  } catch (e) {
    errorMessage.value = "Login failed. Please check your credentials.";
  }


}
</script>

<template>
  <div id="login" class="d-flex justify-content-center align-items-center vh-100">
    <div class="login-form card p-4">
      <h2 class="card-title text-center mb-4">Login</h2>
      <div class="mb-3">
        <input v-model="credentials.email" class="form-control" placeholder="Email" type="email"/>
      </div>
      <div class="mb-3">
        <input v-model="credentials.password" class="form-control" type="password" placeholder="Password"/>
      </div>
      <button @click="login" class="btn btn-primary w-100">Login</button>
      <p v-if="errorMessage" class="error text-danger text-center mt-3">{{ errorMessage }}</p>
    </div>
  </div>
</template>
