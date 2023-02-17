import { createWebHashHistory, createRouter } from 'vue-router';
import CodesList from '@/components/CodesList.vue';
import CodeAddList from '@/components/CodeAddList.vue';

const routes = [
    {
        path: "/",
        name: "CodesList",
        component: CodesList
    },
    {
        path: "/codes-add",
        name: "CodeAddList",
        component: CodeAddList
    }
];

const router = createRouter({
    history: createWebHashHistory(),
    routes
});

export default router;