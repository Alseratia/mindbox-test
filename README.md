# Тестовое задание Mindbox
## Задание 2

Выполненное задание находится в папках Task2 и Task2.Tests.

Юнит-тесты можно запустить командой ``dotnet test``
## Задание 3

### Ход решения:
- У нас kubernetes кластер, в котором пять нод.

Для начала создается некоторый деплоймент с шаблоном контейнера внутри(идентификатор app). А также сервис, который служит единой точкой входа для обращения к контейнерам с меткой app.
- Приложение испытывает постоянную стабильную нагрузку в течение суток без значительных колебаний. 3 пода справляются с нагрузкой.

Для этого устанавливаем количество реплик на значение 3
```yaml
spec:
  replicas: 3
```
- Размещение подов на разных нодах для отказоустойчивости.

Для этого используем параметр podAntiAffinity. Чтобы разместить поды на разных нодах, используем ключ "kubernetes.io/hostname", который говорит о том, что поды должны быть разнесены по разным хостам(т.е. нодам)
```yaml
    spec:
      affinity:
        podAntiAffinity:
          requiredDuringSchedulingIgnoredDuringExecution:
          - labelSelector: # метка идентифицирующая приложение
              matchLabels:
                app: app
            topologyKey: "kubernetes.io/hostname"  # разнести по разным хостам
```
- На первые запросы приложению требуется значительно больше ресурсов CPU, в дальнейшем потребление ровное в районе 0.1 CPU. По памяти всегда “ровно” в районе 128M memory.

На каждый запрос выделяется 0.1 cpu и 128Мб:
```yaml
        resources: 
          requests: # ресурсы, выделяемые на каждый запрос
            cpu: 100m
            memory: 128Mi
```
Также здесь же необходимо установить лимиты, которые сам контейнер может потреблять, чтобы предотвратить, что контейнер будет мешать работе других контейнеров на этом ноде или произойдет неправильное масштабирование. 
Однако, в условиях не даны конкретные цифры нод, поэтому опустим лимиты.

В условиях повышенного потребления cpu первыми запросами, могут понадобиться дополнительные поды, т.к. 3 уже будут не справляться. Для этого создается HorizontalPodAutoscaler, который изменяет количество подов ориентируясь на потребление cpu
(Но пункт с постоянной нагрузкой и необходимыми 3 подами немного противоречит тому, что доп. нагрузка первых запросов может изменить кол-во подов, но да ладно)
```yaml
apiVersion: v1
kind: HorizontalPodAutoscaler
metadata:
  name: app-hpa
spec:
  scaleTargetRef:
    apiVersion: v1
    kind: Deployment
    name: app-deployment
  minReplicas: 3
  maxReplicas: 5 # Максимум 5 нод
  metrics:
    - type: Resource
      resource:
        name: cpu
        target:
          type: Utilization
          averageUtilization: 90
```
- Приложение требует около 5-10 секунд для инициализации. Под не должен обрабатывать запросы до завершения инициализации.

Под считается готовым, если готовы все контейнеры в нем. Достаточно к контейнеру добавить пробу на готовность. Это может быть легкий эндпоинт /health или команда shell.
Проба происходит после 5 секунд от начала инициализации, каждые 5 секунд 3 раза
```yaml
        livenessProbe:
          httpGet:
            path: /health
            port: 80
          initialDelaySeconds: 5
          periodSeconds: 5
          failureThreshold: 3
```

### Полный yaml файл:
```yaml
# Сервис как единая точка доступа к приложению
apiVersion: v1
kind: Service
metadata:
  name: app-service
spec:
  selector:
    app: app
  ports:
  - name: http
    port: 80
    targetPort: 80

# Деплоймент
apiVersion: v1
kind: Deployment
metadata:
  name: app-deployment
spec:
  replicas: 3 # Количество копий подов
  selector:
    matchLabels:
      app: app
    spec:
      affinity:
        podAntiAffinity: # настройки отказоустойчивости
          requiredDuringSchedulingIgnoredDuringExecution:
          - labelSelector: # метка идентифицирующая приложение
              matchLabels:
                app: app
            topologyKey: "kubernetes.io/hostname" # разнести по разным хостам
  # Описание контейнера
  template:
    metadata:
      labels:
        app: app
      containers:
      - image: some-container:latest
        livenessProbe:  # проверка что контейнер запущен и работает (необходим эндпоинт)
          httpGet:
            path: /health
            port: 80
          initialDelaySeconds: 5
          periodSeconds: 5
          failureThreshold: 3
        resources: 
          requests: # ресурсы, выделяемые на каждый запрос
            cpu: 100m
            memory: 128Mi
          # также  тут необходимы лимиты для контейнера
# Автоскейлер
apiVersion: v1
kind: HorizontalPodAutoscaler
metadata:
  name: app-hpa
spec:
  scaleTargetRef:
    apiVersion: v1
    kind: Deployment
    name: app-deployment
  minReplicas: 3
  maxReplicas: 5 # Максимум 5 нод
  metrics:
    - type: Resource
      resource:
        name: cpu
        target:
          type: Utilization
          averageUtilization: 90
```
