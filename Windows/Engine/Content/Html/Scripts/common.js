function disableAllControls() {
    $(":input").attr("disabled", true);
}

function enableAllControls() {
    $(":input").attr("disabled", false);
}

function autoGet() {
    setTextInputValues();
    setCheckboxValues();
}

function setTextInputValues() {
    $("input:text, input:password").each(function(index) {
        $(this).val(engine.getValue(this.id));
    });
}

function setCheckboxValues() {
    $("input:checkbox").each(function (index) {
        var value = engine.getValue(this.id).toLowerCase() === "true";
        $(this).prop("checked", value);

    });
}

function autoPut() {
    getTextInputValues();
    getCheckBoxValues();
}

function getTextInputValues() {
    $("input:text, input:password").each(function(index) {
        engine.setValue(this.id, $(this).val());
    });
}

function getCheckBoxValues() {
    $("input:checkbox").each(function(index) {
        var value = !($(this).prop("checked")) ? "False" : "True";
        engine.setValue(this.id, value);
    });
}

function globalInitialize() {
    console.log("global initialize called");
    autoResolveValues();
    autoAddButtonEvents();
}

function autoResolveValues() {
    $("[data-resolve]").each(function() {
        $(this).text(engine.resolveValue($(this).data("resolve")));
    });
}

function autoAddButtonEvents() {
    $("[data-next-button]").click(returnNextClickHandler);
    $("[data-back-button]").click(returnBackClickHandler);
    $("[data-close-button]").click(returnCloseClickHandler);
    $("[data-help-button]").click(showHelpTopicClickHandler);
}

function returnNextClickHandler() {
    disableAllControls();
    engine.returnNext();
}

function returnBackClickHandler() {
    disableAllControls();
    engine.returnBack();
}

function returnCloseClickHandler() {
    disableAllControls();
    engine.returnClose();
}

function showHelpTopicClickHandler(event, data) {
    var topic = $(event.target).data("help-button");
    engine.showHelpTopic(topic);
}

function configureForInBrowserTesting() {
    if (typeof engine !== "undefined") {
        return;
    }

    console.log("InCert engine not detected. Adding mock methods to simulate running inside InCert engine.");

    engine = {};

    engine.getValue = function (property) {
        return property + " value";
    }

    engine.resolveValue = function(value) {
        return value + " value";
    }

    engine.setValue = function (property, value) {
        console.log("setting " + property + " to " + value);
    }

    engine.returnNext = function () {
        console.log("returning next value");
    }
    
    engine.returnBack = function () {
        console.log("returning back value");
    }

    engine.returnClose = function() {
        console.log("returning close value");
    }

    engine.showHelpTopic = function(topic) {
        console.log("showing help topic " + topic);
    }

}

function raiseEvent(type, data) {
    $(document).trigger(type, data)
}

configureForInBrowserTesting();

ellipsis = {
    'value': ['', '.', '..', '...'],
    'count': 0,
    'run': false,
    'timer': null,
    'element': '.ellipsis',
    'start': function () {
        var t = this;
        this.run = true;
        this.timer = setInterval(function () {
            if (t.run) {
                $(t.element).html(t.value[t.count % t.value.length]).text();
                t.count++;
            }
        }, 500);
    },
    'stop': function () {
        this.run = false;
        clearInterval(this.timer);
        this.count = 0;
    }
}

ellipsis.start();


$(document).ready(function () {
    globalInitialize();
});

$(document).on("task_started", function (event, data) {
    if (!data) { return; }

    if (!data.message || !data.message.length) { return; }

    $("[data-task-started-message]").text(data.message);
});


$(document).on("branch_started", function (event, data) {
    if (!data) { return; }

    if (!data.message || !data.message.length) { return; }

    $("[data-branch-started-message]").text(data.message);
});