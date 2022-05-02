
var app = Vue.createApp({
    data() {
        return{
            host: document.location.protocol + '//' + window.location.host + '/',
            title: "Welcome to ULX",
            ads: [],
            categories: [],
            adId: "",
            adTitle: "",
            adDescription: "",
            adPrice: "",
            adCategoryId: 0,
            adCategoryName: "",
            adCreated:"",
            currentCategory:""
   
    }
    },
    methods: {
        refreshData() {
            axios.get(this.host+"api/ad")
                .then((response)=> {
                    this.ads=response.data;
                 //   console.log(this.ads);

                });
            axios.get(this.host+"api/category")
                .then((response)=> {
                    this.categories=response.data;  
               //     console.log(this.categories);
                });
        },

        addButtonClick() {
            this.modalTitle = "New ad";
            this.adId = 0;
            this.adTitle = "";
            this.adDescription = "";
            this.adCategoryName = this.categories[0].name;
            this.adCategoryId = 0;
            this.adPrice = 0;
        },


        addAd() {
            this.adCategoryId = this.categories.find(category => category.name === this.adCategoryName).id;
            this.errorList.splice(0, this.errorList.length);
            axios.post(this.host+"api/ad",
                    {
                        Id:0,
                        Title:this.adTitle,
                        Description:this.adDescription,
                        Price:this.adPrice,
                        CategoryId:this.adCategoryId,
                        Created:new Date()
                    })
                .then((response)=> {
                    this.refreshData();
                });
            this.isDisabled =true ;
        },
        getImage(id) {
            axios.get(this.host + 'Home/GetPhoto?count=0&id=' + id).
                then((response) => {
                    return response;
                });
        }

    },
    mounted:function() {
        this.refreshData();  

    }
});

app.mount('#app');

