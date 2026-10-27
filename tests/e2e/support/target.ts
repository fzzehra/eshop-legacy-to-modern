// Testlerin hangi uygulamaya karşı çalıştığını belirtir.
// Varsayılan: legacy. Yeni uygulamada: $env:TARGET="modern"
export const TARGET = (process.env.TARGET ?? 'legacy').toLowerCase();
export const isLegacy = TARGET === 'legacy';