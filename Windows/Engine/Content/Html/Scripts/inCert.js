function init() {
    if (document.engineScriptVersion) {
        if (document.engineScriptVersion >= 1.0) {
            return;
        }
    }

    document.engineScriptVersion = 1.0;

    function disableAllControls() {
        $(":input").attr("disabled", true);
    }

    function enableAllControls() {
        $(":input").attr("disabled", false);
    }

    function globalInitialize() {
        console.log("global initialize called");
        resolveValues();
        getInputValues();
        getCheckBoxValues();
        addButtonEvents();
        addChangeEvents();
        configureCloseButton();
        configureCloseQuestion();
        configureInitialVisibility();
        configureInitialEnabledState();
        configureConditionalVisibility();
        configureConditionalEnabledState();
        configureValidation();
    }

    function resolveValues() {
        $("[data-resolve]").each(function () {
            $(this).text(engine.resolveValue($(this).data("resolve")));
        });
    }

    function getInputValues() {
        $("input:text[data-setting], input:password[data-setting]").each(function () {
            var setting = $(this).data("setting");
            $(this).val(engine.getValue(setting));
        });
    }

    function getCheckBoxValues() {
        $("input:checkbox[data-setting]").each(function () {
            var setting = $(this).data("setting");
            var value = engine.getValue(setting).toLowerCase() === "true";
            $(this).prop("checked", value);
        });
    }

    function addButtonEvents() {
        $("[data-setting-button").click(settingButtonClickHandler);
        $("[data-help-button]").click(showHelpTopicClickHandler);

        $("[data-back-button]").click(new genericButtonClickHandler("returnBackResult").handler);
        $("[data-close-button]").click(new genericButtonClickHandler("returnCloseResult").handler);
        $("[data-exit-utility-button]").click(new genericButtonClickHandler("returnExitUtilityResult").handler);
        $("[data-leave-branch-back-button]").click(new genericButtonClickHandler("returnLeaveBranchBackResult").handler);
        $("[data-leave-branch-next-button]").click(new genericButtonClickHandler("returnLeaveBranchNextResult").handler);
        $("[data-next-button]").click(new genericButtonClickHandler("returnNextResult").handler);
        $("[data-repeat-branching-task-result]").click(new genericButtonClickHandler("returnRepeatBranchingTaskResult").handler);
        $("[data-repeat-current-branch-result]").click(new genericButtonClickHandler("returnRepeatCurrentBranchResult").handler);
        $("[data-repeat-current-task-result]").click(new genericButtonClickHandler("returnRepeatCurrentTaskResult").handler);
        $("[data-repeat-parent-branch-result]").click(new genericButtonClickHandler("returnRepeatParentBranchResult").handler);
        $("[data-restart-computer-result]").click(new genericButtonClickHandler("returnRestartComputerResult").handler);

        $("[data-error-button]").click(returnErrorClickHandler);

        $("[data-advanced-menu-button]").click(showAdvancedMenuButtonClickHandler);
        $("[data-stored-result-button").click(returnStoredResultClickHandler);
    }

    function addChangeEvents() {
        $("input:text[data-setting], input:password[data-setting]").change(inputValueChangeHandler);
        $("input:checkbox[data-setting]").change(checkBoxValueChangeHandler);
    }

    function configureCloseButton() {
        if ($(document.body).data("disable-close-button") !=undefined) {
            engine.disableCloseButton(true);
        }

        if ($(document.body).data("enable-close-button") !=undefined) {
            engine.disableCloseButton(false);
        }
    }

    function configureCloseQuestion() {
        if ($(document.body).data("suppress-close-question") != undefined) {
            engine.suppressCloseQuestion(true);
        }

        if ($(document.body).data("raise-close-button") != undefined) {
            engine.suppressCloseQuestion(false);
        }
    }

    function configureInitialVisibility() {
        $("[data-show]").hide();
        $("[data-hide]").show();
    }

    function configureInitialEnabledState() {
        $("[data-enable]").attr("disabled", true);
        $("[data-disable]").attr("disabled", false);
    }

    function configureConditionalVisibility() {
        $("[data-show][data-if-help-topic-exists]").each(function () {
            var topic = $(this).data("help-topic");
            if (engine.helpTopicAvailable(topic)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
        
        $("[data-show][data-if-setting-exists]").each(function () {
            var key = $(this).data("setting");
            if (engine.settingExists(key)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });

        $("[data-hide][data-if-help-topic-exists]").each(function () {
            var topic = $(this).data("help-topic");
            if (engine.helpTopicAvailable(topic)) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });

        $("[data-hide][data-if-setting-exists]").each(function () {
            var key = $(this).data("setting");
            if (engine.settingExists(key)) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    }

    function configureConditionalEnabledState() {
        $("[data-enable][data-if-help-topic-exists]").each(function () {
            var topic = $(this).data("help-topic");
            var available = engine.helpTopicAvailable(topic);
            $(this).attr("disabled", !available);
        });

        $("[data-enable][data-if-setting-exists]").each(function () {
            var key = $(this).data("setting");
            var exists = engine.settingExists(key);
            $(this).attr("disabled", !exists);
        });

        $("[data-disable][data-if-help-topic-exists]").each(function () {
            var topic = $(this).data("help-topic");
            var available = engine.helpTopicAvailable(topic);
            $(this).attr("disabled", available);
        });

        $("[data-disable][data-if-setting-exists]").each(function () {
            var key= $(this).data("setting");
            var available = engine.settingExists(key);
            $(this).attr("disabled", !available);
        });
    }

    function configureValidation() {
        var instances = $("form[data-validate]");
        if (!instances.length) {
            return;
        }

        instances.validate({
            onfocusout: function (element, event) {
                if (isValidationOnSubmitSpecified(this)) {
                    return;
                }
                $(element).valid();
            },
            highlight: function (element) {
                $(element).closest(".form-group").addClass("has-error");
            },
            unhighlight: function (element) {
                $(element).closest(".form-group").removeClass("has-error");
            },
            errorElement: "span",
            errorClass: "help-block",
            errorPlacement: function (error, element) {
                var errorElement = element.parent().find(".error");
                if (errorElement.length > 0) {
                    $(errorElement).replaceWith(error);
                    return;
                }

                if (element.parent(".input-group").length) {
                    error.insertAfter(element.parent());
                } else {
                    error.insertAfter(element);
                }
            },
            showErrors: function (errorMap, errorList) {
                var eventType = errorList.length > 0
                    ? "engine_form_invalid" :
                    "engine_form_valid";

                document.raiseEngineEvent(eventType, {});

                if (!isFormValidationInitializing(this.currentForm)) {
                    this.defaultShowErrors();
                }
            }
        });

        if (isValidationOnSubmitSpecified(this)) {
            return;
        }

        $("form").data("initializing", true);
        $("form").valid();
    }

    function isValidationOnSubmitSpecified(form) {
        var value = $(form).data("validate-on");
        return value !== "change";
    }

    function isFormValidationInitializing(form) {
        var initializing = $(form).data("initializing");
        $(form).removeData("initializing");
        return initializing;
    }

    function validateForm(target) {
        if (!$(target).filter("[data-validate-form]").length) {
            return true;
        }

        return ($("form").valid());
    }

    function settingButtonClickHandler(event) {
        var setting = $(event.target).data("setting");
        var value = $(event.target).data("value");
        setValueSafe(setting, value);
    }

    function showAdvancedMenuButtonClickHandler(event) {
        var group = $(event.target).data("advanced-menu-group");
        disableAllControls();
        engine.showAdvancedMenu(group);
        enableAllControls();
    }

    function returnStoredResultClickHandler(event) {
        if (!validateForm(event.target)) {
            return;
        }

        engine.returnStoredResult($(event.target).data("setting"));

    }

    function inputValueChangeHandler(event) {
        var setting = $(event.target).data("setting");
        setValueSafe(setting, $(event.target).val());
    }

    function checkBoxValueChangeHandler(event) {
        var setting = $(event.target).data("setting");
        var value = !($(this).prop("checked")) ? "False" : "True";
        setValueSafe(setting, value);
    }

    function showHelpTopicClickHandler(event) {
        var topic = $(event.target).data("help-topic");
        engine.showHelpTopic(topic);
    }

    function returnErrorClickHandler(event) {
        var errorType = $(event.target).data("error-type");
        engine.returnError(errorType);
    }

    function getMessageFromData(data) {
        if (!data) { return undefined; }

        if (!data.message || !data.message.length) { return undefined; }

        return data.message;
    }

    function getTaskIdFromData(data) {
        if (!data) { return undefined; }

        if (!data.id || !data.id.length) { return undefined; }

        return data.id;
    }

    function genericButtonClickHandler(method) {
        function handler(event) {
            if (!validateForm(event.target)) {
                return;
            }

            event.stopImmediatePropagation();

            engine[method]();
        }

        return {
            handler: handler
        }
    }

    function setValueSafe(key, value) {
        engine.setValue(key, String(value));
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

        engine.resolveValue = function (value) {
            return value + " value";
        }

        engine.setValue = function (property, value) {
            console.log("setting " + property + " to " + value);
        }

        engine.returnBackResult = function () {
            console.log("returning back result");
        }

        engine.returnCloseResult = function () {
            console.log("returning close result");
        }

        engine.returnExitUtilityResult = function () {
            console.log("returning exit utility result");
        }

        engine.ReturnLeaveBranchBackResult = function () {
            console.log("returning leave-branch-back result");
        }

        engine.ReturnLeaveBranchNextResult = function () {
            console.log("returning leave-branch-next result");
        }

        engine.returnNextResult = function () {
            console.log("returning next result");
        }

        engine.returnRepeatBranchingTaskResult = function () {
            console.log("returning repeat-branching-task result");
        }

        engine.ReturnRepeatCurrentBranchResult = function () {
            console.log("returning repeat-current-branch result");
        }

        engine.ReturnRepeatCurrentTaskResult = function () {
            console.log("returning repeat-current-task result");
        }

        engine.ReturnRepeatParentBranchResult = function () {
            console.log("returning repeat-parent-branch result");
        }

        engine.ReturnRestartComputerResult = function () {
            console.log("returning restart-computer result");
        }

        engine.returnErrorResult = function (errorType) {
            console.log("returning error result (" + errorType + ")");
        }

        engine.returnStoredResult = function (key) {
            console.log("returning stored result for key " + key);
        }

        engine.showAdvancedMenu = function (group) {
            console.log("showing advanced menu (group = '" + group + "')");
        }

        engine.showHelpTopic = function (topic) {
            console.log("showing help topic " + topic);
        }

        engine.helpTopicAvailable = function (topic) {
            console.log("asserting that " + topic + " exists");
            return true;
        }

        engine.settingExists = function(key) {
            console.log("asserting that setting for key " + key + " exsts");
            return true;
        }

        engine.disableCloseButton = function(value) {
            console.log("calling DisableCloseButton with value of " + value);
        }

        engine.suppressCloseQuestion = function (value) {
            console.log("calling SuppressCloseQuestion with value of " + value);
        }

    }

    document.raiseEngineEvent = function (type, data) {
        $(document).trigger(type, data);
    }

    function getEventSelector(taskId) {
        return taskId
            ? ":not([data-task-id]),[data-task-id='" + taskId + "']"
            : ":not([data-task-id])";
    }

    function processTaskEvents(targets, data) {
        var taskId = getTaskIdFromData(data);

        var selector = getEventSelector(taskId);
        var affected = targets.filter(selector);
        if (!affected.length) { return; }

        affected.filter("[data-disable]").attr("disabled", true);
        affected.filter("[data-enable]").attr("disabled", false);
        affected.filter("[data-show]").show();
        affected.filter("[data-hide]").hide();

        var message = getMessageFromData(data);
        if (message) {
            affected.filter("[data-task-message]").text(message);
        }
    }

    function processValidationEvents(targets, isValid) {
        if (targets.length === 0) {
            return;
        }

        targets.filter("[data-on-form-valid").attr("disabled", isValid);
        targets.filter("[data-on-form-invalid").attr("disabled", !isValid);

    }

    function processGlobalTaskMessages(data) {
        var message = getMessageFromData(data);
        if (!message) {
            return;
        }

        var taskId = getTaskIdFromData(data);
        var selector = getEventSelector(taskId);
        var targets = $("[data-task-message]:not([data-on-task-start]):not(data-on-task-finish)")
            .filter(selector);

        console.log(targets);
        targets.text(message);
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

    $(document).on("engine_task_start", function (event, data) {
        processTaskEvents($("[data-on-task-start]"), data);
        processGlobalTaskMessages(data);

    });

    $(document).on("engine_task_finish", function (event, data) {
        processTaskEvents($("[data-on-task-finish]"), data);
        processGlobalTaskMessages(data);
    });

    $(document).on("engine_form_valid", function (event, data) {
        processValidationEvents($("[data-on-form-valid],[data-on-form-invalid]"), true);
    });

    $(document).on("engine_form_invalid", function (event, data) {
        processValidationEvents($("[data-on-form-valid],[data-on-form-invalid]"), false);
    });

}

init();




