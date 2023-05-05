include .env
export $(shell sed 's/=.*//' .env)

stop-all:
	docker-compose stop

reset-db:
	docker-compose stop db
	docker-compose rm -v db


init-db:
	docker-compose build db
	docker-compose up -d db 
	sleep 15
	docker-compose exec db  /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $$SA_PASSWORD -Q "CREATE DATABASE [LuzIngaDb]"
	docker-compose exec db  /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $$SA_PASSWORD -i setup.sql

init-redis:
	docker-compose up -d redis

stop-redis:
	docker-compose stop redis

init-databases: init-db init-redis

init-monitoring:
	docker-compose up -d  grafana influxdb telegraf

init-load-test: init-monitoring
	docker-compose run k6 run /scripts/stages.js