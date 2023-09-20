#!/bin/sh
FILE=./.env
if test -f "$FILE"; then
  rm ./.env
  cp ./.env.example ./.env
else
  cp ./.env.example ./.env
fi