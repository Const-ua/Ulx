
var app = Vue.createApp({
    data() {
        return {
            host: document.location.protocol + '//' + window.location.host + '/',
            ad: "",
            adTitle: "",
            adDescription: "",
            adPrice: "",
            adCategoryId: 0,
            adCategoryName: "",
            adCreated: "",
            modalTitle: "",
            files: [3]
        }
    },
    methods: {
        showAd(id) {
            axios.get(this.host + "Home/getAd/"+id)
                .then((response) => {
                    this.ad = response.data;

                });
            
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
        }

    },
    mounted: function () {
       }
});

app.mount('#app');

