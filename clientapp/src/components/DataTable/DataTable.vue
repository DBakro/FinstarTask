<template>
    <div class="col-5 table-tools">
        <input v-model="search" class="form-control" placeholder="Введите текст для поиска" @input="searchInput" />
    </div>
    <table class="table table-striped table-bordered">
        <thead>
            <tr>
                <th v-for="column of columns" v-bind:key="column" v-bind:class="{ [column.class]: true }">
                    {{ column.title }}
                </th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="row of rows" v-bind:key="row">
                <td v-for="column of columns" v-bind:key="column" v-bind:class="{ [column.class]: true }">
                    {{ row[column.field] }}</td>
            </tr>
            <tr v-if="rows.length == 0">
                <td v-bind:colspan="columns.length" class="text-center">Записи не найдены</td>
            </tr>
        </tbody>
    </table>
    <div v-if="pages.length > 1" class="btn-group btn-group-right" role="group">
        <button v-for="page in pages" v-bind:key="page" @click="changePage(page)" class="btn"
            v-bind:class="(page == this.page) ? 'btn-secondary' : 'btn-outline-secondary'">{{
                page + 1 }}</button>
    </div>
</template>

<script>
import axios from 'axios';

export default ({
    name: "DataTable",
    props: {
        columns: {
            type: Array,
            require: true
        },
        options: {
            type: Object,
            default() {
                return {
                    dataUrl: '',
                    limit: 10
                };
            }
        }
    },
    data() {
        return {
            rows: [],
            total: 0,
            page: 0,
            pages: [],
            search: ""
        }
    },
    methods: {
        _getData() {
            let data = {
                limit: this.options.limit,
                offset: this.page * this.options.limit,
                search: this.search
            };

            axios.get(this.options.dataUrl, { params: data })
                .then((response) => {
                    this.rows = response.data.rows;
                    this.total = response.data.total;
                    this._setPages();
                })
                .catch((error) => {
                    alert(error);
                })
        },
        _setPages() {
            let pagesCount = Math.floor(this.total / this.options.limit) + 1;
            this.pages = [];
            if (pagesCount > 1) {
                this.pages = Array.from(Array(pagesCount).keys())
            }
        },
        changePage(pageNumber) {
            this.page = pageNumber;
            this._getData();
        },
        searchInput() {
            this._getData();
        }
    },
    async mounted() {
        this._getData();
    }
});
</script>

<style>
.btn-group-right {
    float: right;
}
    .table-tools {
        display: inline-block;
        float: right;
        margin-bottom: 5px;
    }
</style>