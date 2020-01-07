#!/bin/sh
#
# Runs NUnit consoile test runner over test assembly.

runner="./packages/NUnit.ConsoleRunner.3.10.0/tools/nunit3-console"
assembly="./bin/Debug/SimpleContainer.Tests.dll"

$runner $assembly