﻿Vue.component('query-flow', {
    props: ['queries', 'model', 'flowName'],
    data: function () {
        return {
            stepNumber: 0,
            loading: false
        };
    },
    template: `
<div>
    <section class="row"
                v-for="query in queries"
                v-if="canShow(query)">

        <div class="col-md-4"
                :class="{'inactive' : !isCurrent(query)}">

            <h3 :id="stepId(query.Number)">
                {{ query.Name }}
                <span class="badge">{{ query.Number }}/{{queries.length}}</span>
            </h3>

            <div v-for="input in query.Inputs"
                    class="form-group">
                <label :for="input.Name">{{ input.Name }}</label>
                <input :type="input.InputType"
                        class="form-control"
                        :id="input.Name"
                        :name="input.Name"
                        v-model="model[input.Name]">
            </div>

            <p>
                <button type="button"
                        class="btn btn-default"
                        :class="{'disabled' : !isCurrent(query)}"
                        :disabled="!isCurrent(query)"
                        v-if="showBackButton(query)"
                        v-on:click="previous(query)">
                    Back
                </button>

                <button type="button"
                        class="btn btn-primary"
                        :disabled="!isCurrent(query)"
                        :class="{'disabled' : !isCurrent(query)}"
                        v-on:click="next(query)">
                    Next
                </button>
            </p>

        </div>

        <div v-if="query.Result"
                class="col-md-8">

            <div v-if="query.Result && query.Result.Error"
                    class="alert alert-danger"
                    role="alert">
                {{ query.Result.Error }}
            </div>

            <div v-if="query.Result"
                    class="panel panel-default pre-scrollable">
                <div class="panel-heading">
                    <template v-if="query.Comments"
                                v-for="comment in query.Comments">
                        {{ comment }}
                        <br />
                    </template>
                    <template v-if="!query.Comments">
                        {{ query.Name }} &ndash; results:
                    </template>
                </div>
                <table class="table">
                    <thead>
                        <tr>
                            <th v-for="heading in query.Result.ColumnHeadings">{{ heading }}</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="row in query.Result.Lines">
                            <td v-for="cell in row">
                                {{ cell }}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>

    </section>

    <section v-if="finishedQueryFlow()"
                :id="stepId(queries.length)"
                class="alert alert-success"
                role="alert">
        <p><strong>That's all!</strong> You've finished this query flow.</p>
        <p>
            <button type="button"
                    class="btn btn-default"
                    v-on:click="previous()">
                Back
            </button>
            <button type="button"
                    class="btn btn-warning"
                    v-on:click="reset()">
                Start again
            </button>
        </p>
    </section>

    <div v-cloak
            v-if="loading"
            class="center-block text-center">
        <img class="loader"
                src="/img/loading1.svg" />
    </div>
</div>`,
    methods:
    {
        submitQuery: function (query) {
            self = this;
            var submission = {
                flowName: self.flowName,
                originalName: query.OriginalName,
                model: self.model
            };

            query.Result = null;
            $.post('/Query/', submission, function (response) {
                self.loading = false;

                //console.log("Response", response);
                query.Result = response;

                if (!query.Result.Error) {
                    self.stepNumber = query.Number + 1;
                    self.scrollToCurrent();
                }
            });
        },
        stepId: function (number) {
            return "step-" + number;
        },
        showBackButton: function (query) {
            return query.Number > 0;
        },
        isCurrent: function (query) {
            return query.Number === this.stepNumber;
        },
        canShow: function (query) {
            return query.Number <= this.stepNumber;
        },
        next: function (query) {
            this.loading = true;
            this.submitQuery(query);
        },
        previous: function (query) {
            if (query) { this.stepNumber = query.Number - 1; }
            else { this.stepNumber -= 1; }
        },
        reset: function () {
            this.loadState();
        },
        finishedQueryFlow: function () {
            return this.stepNumber >= this.queries.length;
        },
        scrollToCurrent: function () {
            self = this;
            window.setTimeout(function () {
                var stepIdTag = "#" + self.stepId(self.stepNumber);
                var $nextHeader = $(stepIdTag);

                $('html, body').animate({
                    scrollTop: $nextHeader.offset().top
                }, 500);
            }, 100);
        }
    },
    mounted: function () {
        this.stepNumber = 0;
    }
});