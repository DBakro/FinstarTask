<template>
    <br />
    <router-link class="btn btn-light inline-element" to="/codes-add">
        <i class="bi bi-plus-lg"></i>&nbsp;Добавать
    </router-link>
    <DataTable :columns="columns" :options="options" />
</template>

<script>
import DataTable from '@/components/DataTable/DataTable.vue'

export default {
    name: 'CodesList',
    components: {
        DataTable
    },
    data() {
        return {
            columns: [
                {
                    name: '№',
                    field: 'id',
                    class: 'text-center col-1',
                    title: 'Порядковый номер'
                },
                {
                    name: 'Код',
                    field: 'code',
                    class: 'text-center col-2'
                },
                {
                    name: 'Значение',
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