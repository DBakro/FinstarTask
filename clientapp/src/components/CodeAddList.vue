<template>
    <br />
    <Form @submit="onSubmit" :initial-values="initialValues">
        <FieldArray name="codes" v-slot="{ fields, push, remove }">
            <button type="button" @click="push({ code: null, value: '' })" class="btn btn-primary add-btn"><i
                    class="bi bi-plus-lg"></i>&nbsp;Добавить код</button>
            <span v-show="dataError.length" class="text-danger">{{ dataError }}</span>
            <div class="col-12" style="max-height: 600px; overflow-y: auto;">
                <table class="table table-striped table-bordered table-scrollable">
                    <thead>
                        <tr>
                            <th class="col-3 text-center required"><label class="form-label">Код</label></th>
                            <th class="col text-center required"><label class="form-label">Значение</label></th>
                            <th v-if="fields.length > 1" width="50" class="text-center"><i class="bi bi-trash"></i></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(entry, idx) in fields" :key="entry.key">
                            <td class="col-3 text-center">
                                <Field :name="`codes[${idx}].code`" type="number" class="form-control"
                                    placeholder="Введите код" :rules="[isRequired, codeRange]" />
                                <ErrorMessage :name="`codes[${idx}].code`" class="text-danger" />
                            </td>
                            <td class="col text-center">
                                <Field :name="`codes[${idx}].value`" type="text" class="form-control"
                                    placeholder="Введите значение" :rules="isRequired" />
                                <ErrorMessage :name="`codes[${idx}].value`" class="text-danger" />
                            </td>
                            <td width="50" v-if="fields.length > 1">
                                <button class="btn btn-light" type="button" title="Удалить строку" @click="remove(idx)">
                                    <i class="bi bi-trash"></i>
                                </button>
                            </td>
                        </tr>

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
        </FieldArray>
    </Form>
</template>

<style>
.add-btn {
    margin-bottom: 5px;
}
</style>

<script setup>

const initialValues = {
    codes: [{ code: null, value: "" }],
};

</script>

<script>

import axios from 'axios'
import { Form, Field, FieldArray, ErrorMessage } from 'vee-validate';

export default {
    name: 'CodeAddList',
    components: {
        Form,
        Field,
        FieldArray,
        ErrorMessage
    },
    data() {
        return {
            submitting: false,
            dataError: ""
        }
    },
    methods: {
        isRequired(value) {
            if (value) {
                return true;
            }
            return 'Поле должно быть заполнено';
        },
        codeRange(value) {
            const minRange = -2147483647;
            const maxRange = 2147483647;

            if (parseInt(value) <= maxRange && parseInt(value) >= minRange) {
                return true;
            }
            return `Код должен быть в диапазоне от ${minRange} до ${maxRange}`;
        },
        onSubmit(values) {
            if (!values.codes.length) {
                this.dataError = "Хотя бы одна строка должна быть заполнена";
                return;
            }

            this.dataError = "";
            this.submitting = true;
            axios({
                method: 'post',
                url: '/Code',
                data: values.codes
            })
                .then(() => {
                    this.$router.push("/");
                })
                .finally(() => {
                    this.submitting = false;
                });
        }
    }
}
</script>