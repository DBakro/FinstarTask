<template>
    <br />
    <form @submit.prevent="handleSubmit">
        <button type="submit" @click="addCode" class="btn btn-primary"><i class="bi bi-plus-lg"></i>&nbsp;Добавить код</button>
        <div class="col-12" style="max-height: 600px; overflow-y: auto;">
            <table class="table table-striped table-bordered table-scrollable">
                <thead>
                    <tr>
                        <th class="col text-center required"><label class="form-label">Код</label></th>
                        <th class="col text-center required"><label class="form-label">Значение</label></th>
                        <th v-if="codes.length > 1" width="50" class="text-center"><i class="bi bi-trash"></i></th>
                    </tr>
                </thead>
                <tbody>
                   <CodeAdd v-for="(code, index) in codes" :key="index" :codeObject="code" :rowCount="codes.length" @click="deleteRow(index)"></CodeAdd>
                </tbody>
            </table>
        </div>
        <hr />
        <div class="row">
            <div class="col">
                <router-link class="btn btn-light" to="/"><i class="bi bi-x-circle"></i>&nbsp;Отмена</router-link>
            </div>
            <div class="col">
                <button type="submit" class="btn btn-success" v-bind:disabled="submitting" style="float:right;"><i
                        class="bi bi-check"></i>&nbsp;Сохранить</button>
            </div>
        </div>
    </form>
</template>

<script>
import axios from 'axios'
import CodeAdd from '@/components/CodeAdd.vue'

export default {
    name: 'CodeAddList',
    components: {
         CodeAdd
    },    
    data() {
        return {
            modelState: {},
            submitting: false,
            codes: [
                {
                    code: null,
                    value: ""
                }
            ]
        }
    },
    methods: {
        addCode() {
            this.codes.push({
                code: null,
                value: ""
            });
        },
        deleteRow(index) {
            this.codes.splice(index, 1);
        },
        handleSubmit() {
            this.submitting = true;
            this.modelState = {};

            axios({
                method: 'post',
                url: '/Code',
                data: this.codes
            })
                .then(() => {
                    this.submitting = false;
                    this.$router.push("/");
                })
                .catch((error) => {
                    this.submitting = false;
                    if (error.response.status == 400) {
                        if (error.response.data.errors) {
                            Object.keys(error.response.data.errors).forEach(k => {
                                this.modelState[k] = error.response.data.errors[k].join(';');
                            })
                        }
                        else {
                            this.modelState = error.response.data;
                        }
                    }
                });
        }
    }
}
</script>