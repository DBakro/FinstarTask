<template>
    <br />
    <router-link class="btn btn-light inline-element" to="/codes-add">
        <i class="bi bi-person-plus-fill"></i>&nbsp;Добавать
    </router-link>
    <DataTable :columns="columns" :options="options" />
</template>

<script>
import DataTable from '@/components/DataTable/DataTable.vue';

export default {
    name: 'CodesList',
    components: {
        DataTable
    },
    data() {
        return {
            columns: [
                {
                    title: 'Порядковый номер',
                    field: 'id',
                    class: 'text-center col-2'
                },
                {
                    title: 'Код',
                    field: 'code',
                    class: 'text-center col'
                },
                {
                    title: 'Значение',
                    field: 'value',
                    class: 'text-center col'
                }
            ],
            options: {
                dataUrl: '/Code',
                limit: 10
            }
        }
    },
}
</script>