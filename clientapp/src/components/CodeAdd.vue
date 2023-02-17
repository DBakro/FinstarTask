<template>
    <br />
    <form @submit.prevent="handleSubmit">
        <button @click="addCode" class="btn btn-primary"><i class="bi bi-person-plus-fill"></i>&nbsp;Добавить код</button>
        <table class="table table-striped table-bordered">
            <thead>
                <tr>
                    <th class="col text-center required"><label class="form-label">Код</label></th>
                    <th class="col text-center required"><label class="form-label">Значение</label></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(code, index) in codes" v-bind:key="code">
                    <td class="col text-center">
                        <input type="text" v-model="code.code" :key="index" class="form-control" placeholder="Введите код" />
                    </td>
                    <td class="col text-center">
                        <input type="text" v-model="code.value" :key="index" class="form-control" placeholder="Введите значение" />
                    </td>
                </tr>
            </tbody>
        </table>
        <hr />
        <div class="row">
            <div class="col">
                <router-link class="btn btn-light" to="/"><i class="bi bi-x-circle"></i>&nbsp;Отмена</router-link>
            </div>
            <div class="col">
                <button type="submit" class="btn btn-success" v-bind:disabled="submitting" style="float:right;"><i class="bi bi-check"></i>&nbsp;Сохранить</button>
            </div>
        </div>
    </form>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'CodeAdd',
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