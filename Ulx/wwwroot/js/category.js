
var app = Vue.createApp({
    data() {
        return{
            host:document.location.protocol+'//'+window.location.host+'/',
            title: "Category list",
            categories:[],
            categoryId:"",
            categoryName:"",
            modalTitle:"",
            errorList:[],
            isDisabled:true
        }
    },
    methods: {
        refreshData() {
            axios.get(this.host+"api/category")
                .then((response)=> {
                    this.categories=response.data;  
                });
        },
        addButtonClick() {
            this.modalTitle = "Add category";
            this.categoryId = 0;
            this.categoryName = "";
            this.clearErrorList();
        },
        editButtonClick(obj) {
            this.modalTitle = "Edit category";
            this.categoryId = obj.id;
            this.categoryName = obj.name;
            this.clearErrorList();
        },

        deleteButtonClick(obj) {
            this.categoryId = obj.id;
            this.categoryName = obj.name;
        },

        addCategory() {
            this.errorList.splice(0, this.errorList.length);
            axios.post(this.host+"api/category",
                    {
                        Id:0,
                        Name:this.categoryName
                    })
                .then((response)=> {
                    this.refreshData();
                });
            this.isDisabled =true ;
        },
        updateCategory() {
            this.errorList.splice(0, this.errorList.length);
            this.checkName();
            if (this.errorList.length > 0) {
                event.preventDefault();
                return false;
            }
            axios.put(this.host+"api/category",
                    {
                        Id:this.categoryId,
                        Name:this.categoryName
                    })
                .then((response)=> {
                    this.refreshData();
                });
            this.isDisabled =true ;
        },

        deleteCategory(id) {
            axios.delete(this.host+"api/category/"+id)
                .then((response)=> {
                    this.refreshData();
                });
        },
        checkName() {
            if (this.categoryName.trim().length === 0) {
                this.errorList.push({ name: "errorName", message: "Category name can't be empty" });
                this.isDisabled = true;
            } else {
                this.errorList.pop(this.errorList.find(errorList => errorList.name === 'errorName'));
                this.isDisabled = false;
            }
        },

        clearErrorList(){
            this.errorList.splice(0, this.errorList.length);
        },

    },
    mounted:function(){
        this.refreshData();  
    }
});

app.mount('#app');

