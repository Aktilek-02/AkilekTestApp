<script setup lang="ts">
import {onMounted, ref} from 'vue'
import axios from 'axios';

defineProps<{ msg: string }>()
type User = {
  Id: number;
  fullname?: string;
  email?: string;
  password?: string;
  roleId?: string;
};

const users = ref<User[]>([]);
const newUser = ref<User>({} as User);

const API_URL = "http://localhost:5208/";
const addNewUser = async () => {
  try {
    const response = await axios.post(`${API_URL}api/FirstApp/AddUser`, newUser.value);
    newUser.value = {} as User;
    console.log(response.data);
  } catch (e) {
    console.log(e);

  }

};
const fetchData = async () => {
  console.log("newUser", newUser.value);
  users.value = (await axios.get(`${API_URL}api/FirstApp/GetInfo`)).data;
  console.log("users", users.value);

}
const deleteUser = async (id: number) => {
  try {
    const response = await axios.delete(`${API_URL}api/FirstApp/DeleteUser?id=${id}`);
    console.log('Delete user response:', response.data);
    await fetchData();
  } catch (error) {
    console.error("Error deleting user:", error);
  }
};
onMounted(async () => {
  await fetchData();
})
</script>

<template>
  
  <div class="container mt-5">
    <h1>{{ msg }}</h1>
    <div class="card p-4">
      <h3>Add New User</h3>
      <div class="mb-3">
        <label for="fullname" class="form-label">Full Name:</label>
        <input id="fullname" v-model="newUser.fullname" class="form-control" placeholder="Enter full name"/>
      </div>
      <div class="mb-3">
        <label for="Email" class="form-label">Email:</label>
        <input id="Email" v-model="newUser.email" class="form-control" placeholder="Enter email"/>
      </div>
      <div class="mb-3">
        <label for="password" class="form-label">Password:</label>
        <input id="password" v-model="newUser.password" type="password" class="form-control" placeholder="Enter password"/>
      </div>
      <div class="mb-3">
        <label for="role" class="form-label">Role:</label>
        <input id="role" v-model="newUser.roleId" type="number" class="form-control" placeholder="Enter role"/>
      </div>
      <button @click="addNewUser" class="btn btn-primary">Add User</button>
    </div>

    <div class="mt-5">
      <h3>User List</h3>
      <ul class="list-group">
        <li v-for="u in users" :key="u.Id" class="list-group-item d-flex justify-content-between align-items-center">
          <span>
            <strong>{{ u.Id }} - {{ u.fullname }}</strong> - {{ u.email }} - Role: {{ u.roleId }}
          </span>
          <button @click="deleteUser(u.Id)" class="btn btn-danger">Delete</button>
        </li>
      </ul>
    </div>
  </div>
</template>

<style scoped>
.read-the-docs {
  color: #888;
}
</style>
