# HyperTimer #

Inspired by [Jon Skeet's](http://stackoverflow.com/users/22656/jon-skeet) [Nodatime](http://nodatime.org/), which came from .Net's DateTime API inconformity, I decided to create a solution to my inconformity with Timers API: HyperTimer.

HyperTimer is a handy Time's Factory & Manipulation Library which allows us to create timers on the fly with finite/infinite repetition cycles, attach them handlers, reduce/increase cycles or running time on the go and aims to improve precision by reducing the time resolution interval using processor's  Query Performance Counter(QPC)[which so far now it's still in progress]. 