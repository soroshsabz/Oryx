ARG DEBIAN_FLAVOR
FROM oryx-node-run-base-${DEBIAN_FLAVOR}
ARG NODE10_VERSION

ENV NODE_VERSION="${NODE10_VERSION}" \
    NPM_CONFIG_LOGLEVEL="info"

RUN groupadd --gid 1000 node \
  && useradd --uid 1000 --gid node --shell /bin/bash --create-home node \
  && ARCH= && dpkgArch="$(dpkg --print-architecture)" \
  && case "${dpkgArch##*-}" in \
    amd64) ARCH='x64';; \
    ppc64el) ARCH='ppc64le';; \
    s390x) ARCH='s390x';; \
    arm64) ARCH='arm64';; \
    armhf) ARCH='armv7l';; \
    i386) ARCH='x86';; \
    *) echo "unsupported architecture"; exit 1 ;; \
  esac \
  && imagesDir=/tmp/oryx/images \
  && $imagesDir/installPlatform.sh nodejs $NODE_VERSION --dir /usr/local --links false \
  && ln -s /usr/local/bin/node /usr/local/bin/nodejs \
  && $imagesDir/runtime/node/installDependencies.sh \
  && rm -rf /tmp/oryx

CMD [ "node" ]


