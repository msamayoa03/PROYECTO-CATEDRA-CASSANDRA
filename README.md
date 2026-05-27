# 🗄️ Proyecto Cátedra — Clúster Cassandra con Docker

Proyecto académico que implementa un clúster de **Apache Cassandra** de 3 nodos usando **Docker Compose**. El clúster está configurado bajo el nombre `studyhub` y permite experimentar con bases de datos distribuidas NoSQL en un entorno local reproducible.

---

## 📋 Tabla de Contenidos

- [Descripción](#descripción)
- [Arquitectura](#arquitectura)
- [Requisitos Previos](#requisitos-previos)
- [Instalación y Uso](#instalación-y-uso)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Configuración del Clúster](#configuración-del-clúster)
- [Comandos Útiles](#comandos-útiles)
- [Autores](#autores)

---

## 📖 Descripción

Este proyecto levanta un clúster de 3 nodos de Apache Cassandra interconectados en una red Docker interna (`cassandra-net`). Cada nodo cuenta con su propio volumen persistente y está expuesto en un puerto diferente del host, lo que facilita la conexión y pruebas individuales.

---

## 🏗️ Arquitectura

```
┌─────────────────────────────────────────────────┐
│               Red: cassandra-net                │
│                                                 │
│  ┌───────────────┐   ┌───────────────┐   ┌───────────────┐  │
│  │  cassandra1   │   │  cassandra2   │   │  cassandra3   │  │
│  │  (Seed Node)  │──▶│  rack2        │──▶│  rack3        │  │
│  │  Port: 9042   │   │  Port: 9043   │   │  Port: 9044   │  │
│  └───────────────┘   └───────────────┘   └───────────────┘  │
└─────────────────────────────────────────────────┘
         Clúster: studyhub | DC: datacenter1
```

| Nodo         | Container      | Puerto Host | Rack  | Rol        |
|--------------|----------------|-------------|-------|------------|
| cassandra1   | cassandra1     | 9042        | rack1 | Seed Node  |
| cassandra2   | cassandra2     | 9043        | rack2 | Node       |
| cassandra3   | cassandra3     | 9044        | rack3 | Node       |

---

## ✅ Requisitos Previos

- [Docker](https://docs.docker.com/get-docker/) (v20.10+)
- [Docker Compose](https://docs.docker.com/compose/install/) (v2.0+)
- Al menos **4 GB de RAM** disponibles (Cassandra es exigente en memoria)

---

## 🚀 Instalación y Uso

### 1. Clonar el repositorio

```bash
git clone https://github.com/msamayoa03/PROYECTO-CATEDRA-CASSANDRA.git
cd PROYECTO-CATEDRA-CASSANDRA
```

### 2. Levantar el clúster

```bash
docker compose up -d
```

> ⚠️ **Importante:** Espera al menos **60–90 segundos** entre el arranque de cada nodo para que Cassandra inicialice correctamente. Se recomienda levantar los nodos uno a uno si hay problemas de arranque:

```bash
docker compose up -d cassandra1
# Espera ~60 segundos
docker compose up -d cassandra2
# Espera ~60 segundos
docker compose up -d cassandra3
```

### 3. Verificar el estado del clúster

```bash
docker exec -it cassandra1 nodetool status
```

Deberías ver los 3 nodos en estado `UN` (Up/Normal).

### 4. Acceder a CQL Shell (cqlsh)

```bash
docker exec -it cassandra1 cqlsh
```

### 5. Detener el clúster

```bash
docker compose down
```

Para eliminar también los volúmenes (datos):

```bash
docker compose down -v
```

---

## 📁 Estructura del Proyecto

```
PROYECTO-CATEDRA-CASSANDRA/
├── docker-compose.yml        # Definición del clúster de 3 nodos
├── backup/
│   └── backup.bat            # Script automático de backup (Windows)
└── README.md                 # Este archivo
```

---

## ⚙️ Configuración del Clúster

| Parámetro               | Valor        |
|-------------------------|--------------|
| `CASSANDRA_CLUSTER_NAME`| studyhub     |
| `CASSANDRA_DC`          | datacenter1  |
| Imagen Docker           | cassandra:latest |
| Red interna             | cassandra-net (bridge) |
| Persistencia            | Volúmenes Docker locales |

---

## 🛠️ Comandos Útiles

```bash
# Ver logs de un nodo
docker logs cassandra1 -f

# Ver el estado del anillo
docker exec -it cassandra1 nodetool ring

# Ver información detallada del nodo
docker exec -it cassandra1 nodetool info

# Crear un keyspace de ejemplo
docker exec -it cassandra1 cqlsh -e "
  CREATE KEYSPACE IF NOT EXISTS studyhub
  WITH replication = {
    'class': 'NetworkTopologyStrategy',
    'datacenter1': 3
  };
"
```

---

## 💾 Backup del Clúster

La carpeta `backup/` contiene un script para Windows que realiza un snapshot automático usando `nodetool`.

### Ejecutar el backup (Windows)

```bash
cd backup
backup.bat
```

El script realiza lo siguiente:
1. Ejecuta `nodetool snapshot` dentro del contenedor activo
2. Guarda un snapshot de todos los keyspaces en `/var/lib/cassandra/data` (dentro del volumen Docker)

### Comandos adicionales de snapshots

```bash
# Ver snapshots disponibles
docker exec -it cassandra1 nodetool listsnapshots

# Limpiar snapshots antiguos
docker exec -it cassandra1 nodetool clearsnapshot
```

---

## 👥 Autores

- **msamayoa03** — [GitHub](https://github.com/msamayoa03)
- **Cesosa503** — [GitHub](https://github.com/Cesosa503)

---

## 📄 Licencia

Este proyecto es de uso académico. Libre para modificar y distribuir con fines educativos.
