/*
 *  Class: AppointmentSuggestor
 *  Author: DucBM
 *  Usage: AppointmentSuggestor.CalcTheBestTour(availableSlots, comingDate, comingTime, chosedSamples)
 *  Return: [{SlotDto, SampleId}]
 */
var AppointmentSuggestor = {
    CalcTheBestTour: function (availableSlots, comingDate, comingTime, chosedSamples) {

        const MAX = 24 * 60 * 60; // total wait time is never exceed 24 hours
        var A = [];
        var tmpA = [];
        var dA = []; // dA[i] = true/false;
        var n = chosedSamples.length;
        var minWait = MAX; // MAX

        var result = [];


        var prepareDataStructure = function () {
            // prepare array A[][]
            var arr = [];
            for (var i = 0; i < n; i++) {
                arr.push([]);
                for (var j = 0; j < availableSlots.length; j++) {
                    var slot = availableSlots[j];
                    if (slot.Date == comingDate && slot.SampleGroupId == chosedSamples[i].SampleGroupId && slot.StartTime > comingTime) {
                        var item = {
                            StartTime: slot.StartTime,
                            FinishTime: slot.FinishTime,
                            SampleGroupId: slot.SampleGroupId,
                            Slot: slot,
                        };
                        arr[i].push(item);
                    } // end if
                } // end for j
            } // end for i
            tmpA = arr;
            // define A[], dA[]
            for (var i = 0; i < n; i++) {
                dA[i] = false;
                A.push([]);
            }
        }

        var permute = function (i) {
            if (i == n) {
                dynamicProgram();
                return;
            }
            for (var j = 0; j < n; j++) {
                if (dA[j] == false) {
                    A[i] = tmpA[j];
                    dA[j] = true;
                    permute(i + 1);
                    dA[j] = false;
                }
            }
        }

        var dynamicProgram = function () {
            var dp = []; // dp: total time for waiting (waiting between slots)
            var trace = [];
            // dp
            for (var i = 0; i < n; i++) {
                dp.push([]);
                trace.push([]);
                for (var j = 0; j < A[i].length; j++) {
                    var dpItem = MAX;
                    var traceItem = -1;
                    if (i == 0) {
                        dpItem = A[i][j].StartTime - comingTime;
                    } else {
                        dpItem = MAX;
                        for (var k = 0; k < dp[i - 1].length; k++) {
                            if (A[i - 1][k].FinishTime < A[i][j].StartTime) {
                                var waitTime = A[i][j].StartTime - A[i - 1][k].FinishTime;
                                var tmpValue = dp[i - 1][k] + waitTime;
                                if (tmpValue < dpItem) {
                                    dpItem = tmpValue;
                                    traceItem = k;
                                }
                            }
                        }
                    }
                    dp[i].push(dpItem);
                    trace[i].push(traceItem);
                }
            }
            // calc res
            var tmpMinWait = MAX;
            var jMinWait = -1;
            for (var j = 0; j < dp[n - 1].length; j++) {
                if (dp[n - 1][j] < tmpMinWait) {
                    tmpMinWait = dp[n - 1][j];
                    jMinWait = j;
                }
            }
            // update result
            if (tmpMinWait < minWait) {
                /*console.log(A);
                console.log(dp);
                console.log(trace);*/
                var tmpResult = [];
                var doTrace = function (i, j) {
                    if (i < 0) {
                        return;
                    }
                    doTrace(i - 1, trace[i][j]);
                    var item = {
                        SlotId: A[i][j].Slot.SlotId,
                        SampleGroupId: A[i][j].Slot.SampleGroupId,
                    }
                    tmpResult.push(item);
                }
                doTrace(n - 1, jMinWait);
                result = tmpResult;
            }
        }

        var setSampleIdsIntoResult = function () {
            if (result == null) {
                return;
            }
            for (var i = 0; i < chosedSamples.length; i++) {
                var sampleId = chosedSamples[i].SampleId;
                var sampleGroupId = chosedSamples[i].SampleGroupId;
                for (var j = 0; j < result.length; j++) {
                    if (result[j].SampleGroupId == sampleGroupId) {
                        if (result[j].SampleId == undefined || result[j].SampleId == null) {
                            result[j].SampleId = sampleId;
                            break;
                        }
                    }
                }
            }
        }

        var solve = function () {
            prepareDataStructure();
            permute(0);
            setSampleIdsIntoResult();
        }

        solve();
        return result;

    }// end define CalcTheBestTour
}