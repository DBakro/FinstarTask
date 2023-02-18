<template>
    <div class="col-5 table-tools">
        <input v-model.trim="search" class="form-control" placeholder="Введите текст для поиска" @input="searchInput" />
    </div>
    <table class="table table-striped table-bordered">
        <thead>
            <tr>
                <th v-for="column of columns" v-bind:key="column" v-bind:class="{ [column.class]: true }"
                    v-bind:title="column.title">
                    {{ column.name }}
                </th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="row of rows" v-bind:key="row" v-show="!loading">
                <td v-for="column of columns" v-bind:key="column" v-bind:class="{ [column.class]: true }">
                    {{ row[column.field] }}</td>
            </tr>
            <tr v-if="!loading && rows.length == 0">
                <td v-bind:colspan="columns.length" class="text-center">Записи не найдены</td>
            </tr>
            <tr v-if="loading">
                <td v-bind:colspan="columns.length" class="text-center">Загрузка...</td>
            </tr>
            <tr v-if="loaderror">
                <td v-bind:colspan="columns.length" class="text-center">Ошибка загрузки</td>
            </tr>
        </tbody>
    </table>
    <div class="btn-toolbar">
        <div v-if="rows.length > 1" class="col-6">Показано {{ rows.length + (page * options.limit) }} записей из {{ total }}
        </div>
        <div class="col-6">
            <div v-if="pages.length > 1" class="btn-group btn-group-right" role="group">
                <button v-for="page in pages" v-bind:key="page" @click="changePage(page)" class="btn"
                    v-bind:class="(page == this.page) ? 'btn-secondary' : 'btn-outline-secondary'">{{
                        page + 1 }}</button>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios'

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
            search: "",
            loading: false,
            loaderror: false
        }
    },
    methods: {
        _getData() {
            this.loading = true;

            let data = {
                limit: this.options.limit,
                offset: this.page * this.options.limit,
                search: this.search
            };

            axios.get(this.options.dataUrl, { params: data })
                .then((response) => {
                    this.loaderror = false;
                    this.rows = response.data.rows;
                    this.total = response.data.total;
                    this._setPages();
                })
                .catch((error) => {
                    alert(error);
                    this.loaderror = true;
                })
                .finally(() => {
                    this.loading = false;
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
            if (pageNumber == this.page) {
                return;
            }
            this.page = pageNumber;
            this._getData();
        },
        searchInput() {
            if (this.search) {
                this.page = 0;
            }

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