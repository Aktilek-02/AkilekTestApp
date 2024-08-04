import { createRouter, createWebHistory } from 'vue-router';
import Login from "../components/Login.vue";
import EditUsers from "../components/EditUsers.vue";

const routes = [
    { path: '/', component: Login },
    { path: '/admin', component: EditUsers },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;
