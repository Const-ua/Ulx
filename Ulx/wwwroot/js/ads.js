
var app = Vue.createApp({
    data() {
        return {
            host: document.location.protocol + '//' + window.location.host + '/',
            title: "Ad list",
            ads: [],
            categories: [],
            adId: "",
            adTitle: "",
            adDescription: "",
            adPrice: "",
            adCategoryId: 0,
            adCategoryName: "",
            adCreated: "",
            modalTitle: "",
            errorList: [],
            files: [3],
            isDisabled: true,
            page: 0,
            sort: "Title_Desc"
        }
    },
    methods: {
        refreshData() {
            axios.get(this.host + "api/ad?sort=Created_Desc")
                .then((response) => {
                    this.ads = response.data;
                });
            axios.get(this.host + "api/category")
                .then((response) => {
                    this.categories = response.data;
                });

        },
        getPage(page) {
            this.page = page;
            axios.get(this.host + "api/ad?sort=" + this.sort + "&page=" + this.page)
                .then((response) => {
                    this.ads = response.data;
                });
        },
        sortTitle() {
            if (this.sort === "Title_Desc") {
                this.sort = "Title_Asc";
            } else {
                this.sort = "Title_Desc";
            }
            axios.get(this.host + "api/ad?sort=" + this.sort + "&page=" + this.page)
                .then((response) => {
                    this.ads = response.data;
                });
        },
        sortCreated() {
            if (this.sort === "Created_Desc") {
                this.sort = "Created_Asc";
            } else {
                this.sort = "Created_Desc";
            }
            axios.get(this.host + "api/ad?sort=" + this.sort + "&page=" + this.page)
                .then((response) => {
                    this.ads = response.data;
                });
        },
        sortPrice() {
            if (this.sort === "Price_Desc") {
                this.sort = "Price_Asc";
            } else {
                this.sort = "Price_Desc";
            }
            axios.get(this.host + "api/ad?sort=" + this.sort + "&page=" + this.page)
                .then((response) => {
                    this.ads = response.data;
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
            this.clearErrorList();
            this.files[0] = "";
            this.files[1] = "";
            this.files[2] = "";
        },
        editButtonClick(obj) {
            this.modalTitle = "Edit ad";
            this.adId = obj.id;
            this.adTitle = obj.title;
            this.adDescription = obj.description;
            this.adPrice = obj.price;
            this.adCategoryId = obj.categoryId;
            this.adCategoryName = obj.categoryName;
            this.adCreated = obj.created;
            this.files[0] = obj.file1;
            this.files[1] = obj.file2;
            this.files[3] = obj.file3;
            this.clearErrorList();
        },

        deleteButtonClick(obj) {
            this.adId = obj.id;
            this.adTitle = obj.title;
        },

        addAd() {
            this.adCategoryId = this.categories.find(category => category.name === this.adCategoryName).id;
            this.errorList.splice(0, this.errorList.length);
            axios.post(this.host + "api/ad",
                {
                    Id: 0,
                    Title: this.adTitle,
                    Description: this.adDescription,
                    Price: this.adPrice,
                    CategoryId: this.adCategoryId,
                    Created: new Date(),
                    Files: this.files
                })
                .then((response) => {
                    this.refreshData();
                });
            this.isDisabled = true;
        },
        updateAd() {
            this.errorList.splice(0, this.errorList.length);
            this.checkTitle();
            this.checkDescription();
            this.checkPrice();
            if (this.errorList.length > 0) {
                event.preventDefault();
                //event.stopPropagation();
                return false;
            }
            this.adCategoryId = this.categories.find(category => category.name === this.adCategoryName).id;
            axios.put(this.host + "api/ad",
                {
                    Id: this.adId,
                    Title: this.adTitle,
                    Description: this.adDescription,
                    Price: this.adPrice,
                    CategoryId: this.adCategoryId,
                    Created: this.created,
                    File1: this.files[0],
                    File2: this.files[1],
                    File3: this.files[2]

                })
                .then((response) => {
                    this.refreshData();
                });
            this.isDisabled = true;
        },

        deleteAd(id) {
            axios.delete(this.host + "api/ad/" + id)
                .then((response) => {
                    this.refreshData();
                });
        },

        checkTitle() {
            var err = this.errorList.find(errorList => errorList.name === 'errorTitle');
            if (err != undefined)
                this.errorList.pop(err);
            var value = this.$refs.adTitle.value;
            if (value.trim().length === 0) {
                this.errorList.push({ name: "errorTitle", message: "Ad title can't be empty" });
                this.isDisabled = true;
            } else {
                if (value.trim().length > 200) {
                    this.errorList.push({ name: "errorTitle", message: "Ad title can't be more then 200 characters" });
                    this.isDisabled = true;
                }
                else {
                    this.errorList.pop(this.errorList.find(errorList => errorList.name === 'errorTitle'));
                    this.isDisabled = false;
                }
            }
        },
        checkDescription() {
            var err = this.errorList.find(errorList => errorList.name === 'errorDescription');
            if (err != undefined)
                this.errorList.pop(err);
            var value = this.$refs.adDescription.value;
            if (value.trim().length === 0) {
                this.errorList.push({ name: "errorDescription", message: "An ad description can't be empty" });
                this.isDisabled = true;
            } else {
                if (value.trim().length > 1000) {
                    this.errorList.push({ name: "errorDescription", message: "Description can't be more then 1000 characters" });
                    this.isDisabled = true;
                }
                else {
                    this.errorList.pop(this.errorList.find(errorList => errorList.name === 'errorDescription'));
                    this.isDisabled = false;
                }
            }
        },
        checkPrice() {
            var err = this.errorList.find(errorList => errorList.name === 'errorPrice');
            if (err != undefined)
                this.errorList.pop(err);
            var value = this.$refs.adPrice.value;
            if (value <= 0) {
                this.errorList.push({ name: "errorPrice", message: "Price must be grater then 0." });
                this.isDisabled = true;
            } else {
                this.errorList.pop(this.errorList.find(errorList => errorList.name === 'errorPrice'));
                this.isDisabled = false;
            }
        },

        getFile1() {
            this.files[0] = document.getElementById("File1").files[0].name;

            var img = this.$refs.image1;
            var input = document.getElementById("File1");
            var fReader = new FileReader();
            fReader.readAsDataURL(input.files[0]);
            fReader.onloadend = function (event) {
                img.src = event.target.result;


            }
            this.isDisabled = false;
        },
        getFile2() {
            this.files[0] = document.getElementById("File2").files[0].name;;

            var img = this.$refs.image2;
            var input = document.getElementById("File2");
            var fReader = new FileReader();
            fReader.readAsDataURL(input.files[0]);
            fReader.onloadend = function (event) {
                img.src = event.target.result;

            }
            this.isDisabled = false;
        },
        getFile3() {
            this.files[0] = document.getElementById("File3").files[0].name;;

            var img = this.$refs.image3;
            var input = document.getElementById("File3");
            var fReader = new FileReader();
            fReader.readAsDataURL(input.files[0]);
            fReader.onloadend = function (event) {
                img.src = event.target.result;

            }
            this.isDisabled = false;
        },

        validateForm() {
            this.errorList.splice(0, this.errorList.length);
            this.checkTitle();
            this.checkDescription();
            this.checkPrice();
            if (this.errorList.length > 0) {
                event.preventDefault();
                event.stopPropagation();
                this.isDisabled = true;
                return false;
            }
        },



        clearErrorList() {
            this.errorList.splice(0, this.errorList.length);
        }

    },
    mounted: function () {
        this.refreshData();
    }
});

app.mount('#app');

