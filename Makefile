up: up-db restart-api

restart-api:
	docker compose -f docker-compose.api.yml up -d --force-recreate

up-db:
	docker compose -f docker-compose.database.yml up -d --force-recreate
