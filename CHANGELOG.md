# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [0.6.9] - 2020-01-08
Initial published release.

## [0.7.0] - 2020-02-04
* Add example project
* Add `TypeNotResolvedException` explanation to message
* Remove redundant method `Resolve()`
* Implement late binding
* Add multiple `Inject` attributes registration
* Add message generation for pull requests

## [0.7.1-beta] - 2020-03-16
* Add dependency graph output on circular dependency
* Add dependency graph output on not registered typede
* Remove deprecated `Inject` attribute
* Remove `Dispatcher` signal bus
* Replace internal linking with a fully new ones

## [0.7.1-beta.1] - 2020-05-04
* Fix circular dependencies handling
* Implement container override behaviour