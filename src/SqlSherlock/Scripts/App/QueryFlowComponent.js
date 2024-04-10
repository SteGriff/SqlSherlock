Vue.component('query-flow', {
    props: ['flow', 'model', 'connectionName', 'needsRefresh'],
    data: function () {
        return {
            loading: false,
            expanded: false
        };
    },
    computed: {
        queries: function () {
            return this.flow.Queries;
        },
        visibleQueries: function () {
            return this.flow.Queries.filter(q => q.Number <= this.flow.StepNumber)
        }
    },
    methods:
    {
        submitQuery: function (query) {
            self = this;

            // Create null model member for those left unspecified
            // This enables queries with nullable params to work
            for (const input of query.Inputs)
            {
                const inputKey = input.Name.toLowerCase();
                if (!this.model[inputKey]) {
                    this.model[inputKey] = null;
                }
            }

            const submission = {
                flowName: self.flow.Name,
                originalName: query.OriginalName,
                connectionName : self.connectionName,
                model: self.model
            };

            console.log(submission);

            query.Result = null;
            query.Expanded = false;
            $.post('Query/', submission, function (response) {
                self.loading = false;
                query.Result = response;
                query.RunOn = '' + self.connectionName; //Copy string
                self.$emit('run', self.connectionName);

                if (!query.Result.Error) {
                    self.flow.StepNumber = query.Number + 1;
                    self.scrollToCurrent();
                }
            }).fail(function (x) {
                self.loading = false;
                query.Result = { 'Error': x.statusText };
            });
        },
        stepId: function (number) {
            return "step-" + number;
        },
        showBackButton: function (query) {
            return query.Number > 0;
        },
        isCurrent: function (query) {
            return query.Number === this.flow.StepNumber;
        },
        isDate: function (input) {
            return input.InputType.indexOf('datetime') !== -1;
        },
        next: function (query) {
            this.loading = true;
            this.submitQuery(query);
        },
        previous: function (query) {
            if (query) { this.flow.StepNumber = query.Number - 1; }
            else { this.flow.StepNumber -= 1; }
        },
        reset: function () {
            this.$emit('reset');
        },
        finishedQueryFlow: function () {
            return this.flow.StepNumber >= this.queries.length;
        },
        scrollToCurrent: function () {
            self = this;
            window.setTimeout(function () {
                const stepIdTag = "#" + self.stepId(self.flow.StepNumber);
                const $nextHeader = $(stepIdTag);

                $('html, body').animate({
                    scrollTop: $nextHeader.offset().top
                }, 500);
            }, 100);
        },
        toggleExpand: function () {
            this.expanded = !this.expanded;
        }
    },
    mounted: function () {
        this.flow.StepNumber = 0;
    },
    template: `
<div>
    <section class="row"
        v-for="query in visibleQueries">

        <div class="col-md-4"
                :class="{'inactive' : !isCurrent(query)}">

            <h3 :id="stepId(query.Number)">
                {{ query.Name }}
                <span class="badge">{{ query.Number }}/{{queries.length}}</span>
            </h3>

            <div v-for="input in query.Inputs"
                    class="form-group">
                <label :for="input.Name">{{ input.Name }}</label>

                <vue-datepicker-local
                    v-if="isDate(input)"
                    v-model="model[input.Name.toLowerCase()]">
                </vue-datepicker-local>

                <input v-if="!isDate(input)"
                        class="form-control"
                        :type="input.InputType"
                        :id="input.Name"
                        :name="input.Name"
                        v-model="model[input.Name.toLowerCase()]">
            </div>

            <p>
                <button type="button"
                        class="btn btn-default"
                        :class="{'disabled' : !isCurrent(query)}"
                        :disabled="!isCurrent(query)"
                        v-if="showBackButton(query)"
                        @click="previous(query)">
                    Back
                </button>

                <button type="button"
                        class="btn btn-primary"
                        :disabled="!isCurrent(query)"
                        :class="{'disabled' : !isCurrent(query)}"
                        @click="next(query)">
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
                    class="panel"
                    :class="{'panel-default' : !expanded, 'pre-scrollable' : !expanded}">
                <div class="panel-heading">
                    <template v-if="query.Comments">
                        <template v-for="(comment, index) in query.Comments">
                            {{ comment }} <br v-if="index + 1 < query.Comments.length" />
                        </template>
                    </template>
                    <template v-else>
                        {{ query.Name }} &ndash; results:
                    </template>
                    <span class="badge">on {{query.RunOn}}</span>
                    <button type="button" class="btn btn-xs btn-default pull-right" @click="toggleExpand()">
                        <span class="glyphicon glyphicon-resize-full" v-if="!expanded"></span>
                        <span class="glyphicon glyphicon-resize-small" v-else></span>
                    </button>
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
                    @click="previous()">
                Back
            </button>
            <button type="button"
                    class="btn btn-warning"
                    @click="reset()">
                Start again
            </button>
        </p>
    </section>

    <div v-cloak
            v-if="loading"
            class="center-block text-center">
        <img class="loader"
                src="img/loading1.svg" />
    </div>
</div>`
});