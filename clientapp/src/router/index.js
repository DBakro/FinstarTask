import { createWebHashHistory, createRouter } from 'vue-router';
import CodesList from '@/components/CodesList.vue';
import CodeAdd from '@/components/CodeAdd.vue';

const routes = [
    {
        path: "/",
        name: "CodesList",
        component: CodesList
    },
    {
        path: "/codes-add",
        name: "CodeAdd",
        component: CodeAdd
    }
];

const router = createRouter({
    history: createWebHashHistory(),
    routes
});

export default router;