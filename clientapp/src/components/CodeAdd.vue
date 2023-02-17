<template>
    <tr>
        <td class="col text-center">
            <input type="number" v-model.number="code" class="form-control" placeholder="Введите код" />
        </td>
        <td class="col text-center">
            <input type="text" v-model.trim="value" class="form-control" placeholder="Введите значение" />
        </td>
        <td width="50">
            <button v-show="showDelete" class="btn btn-light" type="button" title="Удалить строку" @click="$emit(deleteRow)">
                <i class="bi bi-trash"></i>
            </button>
        </td>
    </tr>
</template>
<script>

export default {
    name: "CodeAdd",
    props: {
        codeObject: {
            type: Object
        },
        rowCount: {
            type: Number
        }
    },
    data() {
        return {
            code: this.codeObject.code,
            value: this.codeObject.value
        }
    },
    computed:{
        showDelete(){
            return this.rowCount > 1;
        }
    }

}

</script>