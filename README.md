# GitSimulator

GitSimulator es una simulación básica de algunas funcionalidades de Git en C#. Utiliza estructuras de datos y métodos para manejar archivos, commits y operaciones como commit y push.

## Funcionalidades

1. **Agregar archivos al área de preparación (staging area)**:
  - `add <filename>`: Agrega un archivo al área de preparación.

2. **Realizar commits**:
  - `commit <message>`: Realiza un commit con los archivos en el área de preparación y un mensaje descriptivo.

3. **Hacer push**:
  - `push`: Simula el envío de los commits locales a un servidor remoto.

4. **Ver el historial de commits**:
  - `log`: Muestra una lista de todos los commits realizados hasta el momento.

5. **Crear y listar ramas**:
  - `branch <branchname>`: Crea una nueva rama.
  - `branch list`: Lista las ramas creadas.

6. **Ayuda**:
  - `help`: Muestra una breve descripción de cada comando y su uso.

7. **Salir**:
  - `exit`: Sale de la aplicación.

## Instrucciones para Ejecutar

1. Clona el repositorio o descarga los archivos.
2. Navega hasta el directorio del proyecto.
3. Ejecuta `docker build -t sisorg-git -f Dockerfile .` para construir la imagen de Docker.
4. Ejecuta `docker run -it sisorg-git` para iniciar la aplicación.
5. Ejecuta en otra consola `docker exec -ti $(docker ps --filter "ancestor=sisorg-git" --format "{{.Names}}") /bin/bash` para poder ingresar al root del proyecto y analizar la carpeta de *WorkArea*.

## Ejemplo de Uso

```shell
git-sisorg> init
Initialization completed.

git-sisorg> add ipsum.txt
Destination directory WorkArea/.git/objects/master does not exist. Creating directory...
File ipsum.txt added to staging area.

git-sisorg> commit "Added ipsum file."
Committed with message: "Added ipsum file."

git-sisorg> status
New files:
  payload.json
  payload1.json
  payload2.json
  ipsum2.txt
StagingArea:
  gitSimulatorState.json
  objects/master/ipsum.txt

git-sisorg> add payload.json
File payload.json added to staging area.

git-sisorg> commit "Added payload file."
Committed with message: "Added payload file."

git-sisorg> log
commit 63c1727392ef34d53981a96252816669e09e3b99 in >> (master)
Author: Sisorg
Date: 06/06/2024 02:28:53

      Files: payload.json

commit 31f2aaa81eacd63ca705c8a766ee7652f2fc8a7d in >> (master)
Author: Sisorg
Date: 06/06/2024 02:26:44

      Files: ipsum.txt

git-sisorg> push
Commits pushed to remote repository.

git-sisorg> branch develop
Destination directory WorkArea/.git/objects/develop does not exist. Creating directory...
Branch develop created.

git-sisorg> branch list
Available branches:
  master
  develop

git-sisorg> add ipsum2.txt
File ipsum2.txt added to staging area.

git-sisorg> commit "Added ipsum2 file."
Committed with message: "Added ipsum2 file."

git-sisorg> log
commit 4fde4c205b80062abca31cd3d04a214a6dcaa391 in >> (develop)
Author: Sisorg
Date: 06/06/2024 02:30:50

      Files: ipsum2.txt
      
git-sisorg> help
-----------------------------------------------------------------
Available commands:
init               - Initialize the application.
status             - Show the information
add <filename>     - Add file to staging area
commit <message>   - Commit changes in staging area with message
push               - Push commits to remote repository
log                - Show commit history
branch <name>      - Create new branch
branch list        - List all branches
help               - Show help
exit               - Exit the application
-----------------------------------------------------------------
```