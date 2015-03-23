(function () {
    function init() {
        $(".ellipsis").each(function () {
            var instance = new ellipses(this);
            $(this).data("ellipsis-instance", instance);
            instance.start();
        });
    }

    $(document).ready(function () {
        init();
    });

    function ellipses(element) {
        return {
            value: ["", ".", "..", "..."],
            count: 0,
            run: false,
            timer: null,
            start: function () {
                var t = this;
                this.run = true;
                this.timer = setInterval(function () {
                    if (t.run) {
                        $(element).html(t.value[t.count % t.value.length]).text();
                        t.count++;
                    }
                }, 500);
            },
            stop: function () {
                this.run = false;
                clearInterval(this.timer);
                this.count = 0;
                $(element).text("");
            }
        }
    }
})();