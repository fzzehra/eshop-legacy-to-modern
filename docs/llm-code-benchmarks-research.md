# LLM Kod Benchmark'ları Araştırması

Kayıt tarihi: 28.10.2026 · Kaydeden: İrem · Görev dağılımı madde 10

## 1. Amaç

Bu araştırma şu üç soruyu cevaplamayı hedefliyor:

1. Codex, Claude gibi LLM'lerin kod yazma yeteneğini ölçmek için kullanılan
   benchmark'lar tam olarak neyi ölçüyor?
2. Hocanın bahsettiği "Java yarışmaları" muhtemelen neye karşılık geliyor?
3. Kodun sadece **doğru** değil, **ne kadar iyi** yazıldığını ölçen çalışmalar
   hangileri ve hangi ölçütleri kullanıyorlar?

Sonuç bölümünde, bu bulgulardan türetilen ve Zehra'nın topladığı kalite kriterleri
listesine (madde 12) girdi olacak somut bir öneri listesi var.

## 2. LLM Kod Benchmark'ları Ne Ölçüyor?

Mevcut benchmark'ların büyük çoğunluğu **doğruluk/functional correctness** odaklı:
model ürettiği kod, önceden yazılmış unit testlerden geçiyor mu? Ama neyi test
ettikleri ve ne ölçekte kod istedikleri birbirinden çok farklı.

| Benchmark | Ne ölçüyor | Kapsam |
|---|---|---|
| **HumanEval** | İzole fonksiyon üretimi: docstring'den tek bir fonksiyon yazma | 164 problem, çoğu 20 satırın altında; döngü/koşul/liste işlemleri gibi temel beceriler |
| **SWE-bench (Verified / Pro)** | Gerçek bir GitHub issue'sunu, mevcut bir kod tabanında çok dosyalı değişiklikle çözme | 2.294 problem (gerçek Python projeleri); mimariyi anlama, API sözleşmelerine uyma, regresyon yaratmama. **Pro** sürümü daha az kontamine ve daha gerçekçi — aynı modellerin Verified'da yüksek, Pro'da çok daha düşük skor alması bunun göstergesi |
| **LiveCodeBench** | "Taze" (henüz eğitim verisine karışmamış) problemlerle kontaminasyonsuz değerlendirme | LeetCode/AtCoder/Codeforces'tan aylık güncellenen problemler; ezber değil gerçek üretim kapasitesini ölçmeyi hedefliyor |

**Codex/Claude karşılaştırması (Ekim 2026 itibarıyla, SWE-bench Verified lider
tablosundan):** Claude Opus 5 %96 ile listeyi açıyor; bir önceki nesilde (Opus 4.x)
GPT-5 serisi (Codex) bazı sürümlerde Claude'u geçmişti (ör. GPT-5.3 Codex %85 vs
Opus 4.5 %80,9). Daha önemlisi: **aynı model** Verified'da yüksek, Pro'da çok daha
düşük skorluyor (Opus 4.5: Verified %80,9 → Pro %45,9). Bu, "hangi model kazandı"
sorusunun tek bir sayıyla cevaplanamayacağını, benchmark'ın kontaminasyona
dayanıklılığının skoru doğrudan etkilediğini gösteriyor.

**Çıkarım:** "Kod benchmark'ı" tek bir şey değil — izole fonksiyon mu (HumanEval),
gerçek repo'da çok dosyalı değişiklik mi (SWE-bench), taze/kontaminasyonsuz problem
mi (LiveCodeBench) ölçüldüğüne göre tamamen farklı yetenekler test ediliyor. Bizim
projemizde (legacy→modern dönüşüm) en yakın analog **SWE-bench** tipi görevler:
mevcut bir kod tabanını anlayıp değiştirmek, tek bir fonksiyon yazmak değil.

## 3. "Java Yarışmaları" — Muhtemel Referans

Hocanın bahsettiği "Java yarışmaları" en olası şekilde **ICPC (International
Collegiate Programming Contest)**'e işaret ediyor — üniversiteler arası en büyük
rekabetçi programlama yarışması, geleneksel olarak Java/C++/Python kabul ediyor ve
2026 başında geniş yankı uyandıran bir AI haberiyle gündemdeydi:

- **ICPC World Finals 2025** (Bakü, Eylül 2025): OpenAI'nin modeli 12 problemin
  **12'sini de** 5 saat içinde çözdü — katılan 139 üniversite takımının hepsinden
  önde bitirdi (en iyi insan takımı St. Petersburg State University, 11 problem).
  Google DeepMind'ın Gemini 2.5 Deep Think modeli 10/12 çözerek altın madalya
  seviyesine ulaştı, hatta hiçbir insan takımının çözemediği bir problemi çözdü.

Bu, "modeller artık doğru kod üretebiliyor mu" sorusunun rekabetçi programlama
bağlamında neredeyse çözülmüş sayılabileceğini gösteriyor — ama ICPC problemleri de
HumanEval gibi **izole, kendi içinde tam problemler** (gerçek bir kod tabanını
anlama/değiştirme değil). Benzer, Java'ya özgü akademik benchmark'lar da var:

- **JavaBench / Java Academic Benchmark (JAB):** Nesne yönelimli (OOP) Java
  yeteneğini ölçmek için üniversite sınavlarından/projelerinden derlenmiş;
  JavaBench 4 proje, 389 metot içeriyor, JAB ise 10 yıllık gerçek sınav sorularını
  kullanıyor.
- **Code4Bench:** Codeforces yarışma verisinden (28 dil, Java dahil)
  derlenmiş, 3,4 milyon program içeren dev bir veri seti — program analiz
  tekniklerini karşılaştırmak için kullanılıyor.
- **OJBench:** NOI ve ICPC'den 232 yarışma problemiyle, modellerin "yarışma
  seviyesi" kod akıl yürütmesini ölçüyor.

**Not:** Hocanın tam olarak hangi haberi/kaynağı kastettiği bu oturumda doğrulanamadı;
yukarıdaki ICPC 2025 sonucu en çok konuşulan ve zaman olarak en örtüşen aday, ama
toplantıda netleştirilmesi gerekebilir.

## 4. Kodun "Ne Kadar İyi" Yazıldığını Ölçen Çalışmalar

Klasik benchmark'lar ikili bir mantıkla çalışıyor: testten geçti mi, geçmedi mi.
Ama testten geçen iki kod parçası; satır sayısı, döngüsel karmaşıklık (cyclomatic
complexity), isimlendirme kalitesi veya stil ihlali sayısı bakımından çok farklı
olabilir. Bunu ölçmeye çalışan yakın tarihli çalışmalar:

- **RACE benchmark** (*Beyond Correctness: Benchmarking Multi-dimensional Code
  Generation for LLMs*) — kodu dört boyutta değerlendiriyor: **R**eadability,
  m**A**intainability, **C**orrectness, **E**fficiency. Okunabilirlik için AST
  analizi + kod uzunluğu/isimlendirme/yorum yoğunluğu gibi sezgisel ölçütler
  kullanılıyor. Önemli bulgular:
  - Doğru (testten geçen) kodlar bile satır sayısı, karmaşıklık ve stil ihlali
    açısından büyük farklar gösterebiliyor.
  - Modellerin çoğu belirli bir kodlama stiline "önyargılı" — kullanıcının
    istediği kısıtlara (ör. belirli bir stil, belirli bir verimlilik hedefi)
    uyum sağlamakta zorlanıyorlar.
- **"Quality Assurance of LLM-generated Code: Addressing Non-Functional Quality
  Characteristics"** — fonksiyonel olmayan kalite nitelikleri (bakım
  yapılabilirlik, güvenlik, stil) için statik analiz araçlarının (SonarQube,
  CodeQL, PMD) nasıl kullanıldığını inceliyor. Statik ölçütlerin bakım
  yapılabilirliğin yalnızca bir kısmını yakaladığını, uzun vadeli bakım
  sorunlarının çoğunu kaçırabileceğini vurguluyor.
- **"Evaluating LLM-Generated Code: A Benchmark and Developer Study"** —
  otomatik metriklerin yanına gerçek geliştiricilerin değerlendirmesini
  (developer study) ekleyerek, salt statik analiz skorlarının insan algısıyla
  her zaman örtüşmediğini gösteriyor.

**Çıkarım:** "Ne kadar iyi" sorusunun endüstri standardı tek bir cevabı yok; ama
tekrar eden dört eksen var: **okunabilirlik, bakım yapılabilirlik, verimlilik,
güvenlik/statik analiz bulguları** — hepsi "testten geçti mi" sorusunun ötesinde.

## 5. Bizim Projeyle İlişkisi

eShop dönüşüm projesi tam olarak bu literatürün pratik bir örneği: legacy Web
Forms kodunu LLM yardımıyla Blazor'a taşıyoruz ve "doğru çalışıyor mu" dışında
"eskisinden daha mı iyi/bakımı daha mı kolay" sorusunu da soruyoruz (bkz. ölçüm
planındaki madde 5 — satır sayısı, hata oranı, code coverage, statik kalite
taraması). Yani takımın zaten planladığı ölçümler (LOC, code coverage,
statik/güvenlik taraması) literatürdeki "readability/maintainability" eksenine denk
düşüyor; sadece "regression testler geçti mi" (correctness) ekseni tek başına
yeterli değil.

## 6. Kalite Kriterleri İçin Öneri Listesi (madde 12'ye girdi)

Zehra'nın toparlayacağı kalite kriterleri listesine, bu araştırmadan önerim:

1. **Doğruluk (correctness):** Regression testlerinin geçme oranı — zaten
   planlanıyor (Zehra/Sinem'in Playwright testleri).
2. **Değişiklik kapsamı/gerçekçilik:** Değişikliğin SWE-bench tipi mi
   (gerçek, çok dosyalı bir kod tabanında) yoksa izole bir fonksiyon mu
   olduğu — bizim projede her zaman ilki, bu yüzden "tek fonksiyon testi"
   gibi basit ölçütler yanıltıcı olabilir.
3. **Okunabilirlik:** İsimlendirme tutarlılığı, fonksiyon/metot uzunluğu,
   yorum yoğunluğu (RACE benchmark'ın kullandığı sezgisel ölçütlere benzer).
4. **Bakım yapılabilirlik:** Statik analiz bulgu sayısı (SonarCloud/CodeQL —
   zaten planda var, Aslı Ceren'in görevi), döngüsel karmaşıklık.
5. **Verimlilik:** Yanıt süresi, istek/saniye, CPU/bellek — zaten ölçüm
   planında var (madde 5).
6. **Kontaminasyon/gerçekçilik farkındalığı:** Kalite iddialarını tek bir
   skora değil, birden fazla boyuta (örn. SWE-bench Verified vs Pro farkı
   gibi) dayandırmak — "testler geçti" tek başına yeterli kanıt değil.

## Kaynaklar

- [LLM Coding Benchmarks: The Complete Guide](https://www.openlayer.com/blog/llm-coding-benchmarks-complete-guide)
- [Large Language Models for Code Generation: A Comprehensive Survey](https://arxiv.org/pdf/2503.01245)
- [SWE-bench Leaderboard 2026](https://codeant.ai/blogs/swe-bench-scores)
- [SWE-bench 2026: Claude Opus 4.6 vs GPT-5.4 Coding Benchmarks](https://evolink.ai/blog/swe-bench-verified-2026-claude-vs-gpt)
- [SWE-bench Verified Leaderboard (October 2026)](https://benchlm.ai/benchmarks/swe-bench-verified)
- [OpenAI and DeepMind Top World's Biggest Coding Contest (ICPC 2025)](https://www.techrepublic.com/article/openai-deepmind-icpc-2025-results/)
- [Gemini achieved gold-medal performance at ICPC World Finals](https://blog.google/innovation-and-ai/models-and-research/google-deepmind/gemini-gold-icpc/)
- [OpenAI outperforms humans and Google at ICPC](https://the-decoder.com/openai-outperforms-humans-and-google-at-the-worlds-top-collegiate-programming-contest/)
- [Beyond Correctness: Benchmarking Multi-dimensional Code Generation for LLMs (RACE)](https://arxiv.org/abs/2407.11470)
- [Quality Assurance of LLM-generated Code: Addressing Non-Functional Quality Characteristics](https://arxiv.org/pdf/2511.10271)
- [Evaluating LLM-Generated Code: A Benchmark and Developer Study](https://arxiv.org/pdf/2605.09059)
- [Code4Bench: A multidimensional benchmark of Codeforces data](https://www.sciencedirect.com/science/article/abs/pii/S1045926X18302489)
- [JavaBench: A Benchmark of Object-Oriented Code Generation for Evaluating LLMs](https://arxiv.org/abs/2406.12902)
- [Java academic benchmark: Exam-based evaluation of LLMs on OOP](https://www.sciencedirect.com/science/article/pii/S0164121226002669)
- [OJBench: A Competition Level Code Benchmark For LLMs](https://arxiv.org/pdf/2506.16395)
