document.addEventListener("DOMContentLoaded", () => {
    const pokemonList = document.getElementById("pokemonList");
    const pokemonDetails = document.getElementById("pokemonDetails");
    const loading = document.getElementById("loading");
    const errorText = document.getElementById("errorText");
  
    const pokemonNameEl = document.getElementById("pokemonName");
    const pokemonImg = document.getElementById("pokemonImg");
    const pokemonTypes = document.getElementById("pokemonTypes");
    const pokemonStats = document.getElementById("pokemonStats");
    const flavorText = document.getElementById("pokemonFlavorText");
  
    function capitalize(str) {
        return str.charAt(0).toUpperCase() + str.slice(1);
    }
  
    function showLoading() {
        loading.style.display = "block";
        errorText.style.display = "none";
        pokemonDetails.style.display = "none";
    }
  
    function hideLoading() {
        loading.style.display = "none";
    }
  
    function showError() {
        errorText.style.display = "block";
        pokemonDetails.style.display = "none";
    }

    function cleanFlavorText(text) {
        return text
            .replace(/\f/g, '\n')
            .replace(/\u00ad\n/g, '')
            .replace(/\u00ad/g, '')
            .replace(/ -\n/g, ' - ')
            .replace(/-\n/g, '-')
            .replace(/\n/g, ' ');
    }
      
  
    
    async function loadPokemonList() {
        showLoading();
        try {
            const res = await fetch("https://pokeapi.co/api/v2/pokemon-species?limit=151");
            const data = await res.json();
            pokemonList.innerHTML = "";
  
            data.results.forEach((species) => {
                const li = document.createElement("li");
                li.textContent = capitalize(species.name);
                loadPokemonData("bulbasaur");
                li.addEventListener("click", () => loadPokemonData(species.name));
                pokemonList.appendChild(li);
            });
        }     
        catch (err) {
            hideLoading();
            showError();
        }
    }
  
    async function loadPokemonData(name) {
        showLoading();
        try {
            const speciesRes = await fetch(`https://pokeapi.co/api/v2/pokemon-species/${name}`);
            const speciesData = await speciesRes.json();
    
            const defaultVariety = speciesData.varieties.find((v) => v.is_default);
            const pokemonRes = await fetch(defaultVariety.pokemon.url);
            const pokemonData = await pokemonRes.json();
  
            pokemonNameEl.textContent = capitalize(pokemonData.name);

            pokemonImg.src = pokemonData.sprites.front_default;
            pokemonImg.alt = pokemonData.name;
  
            pokemonImg.onload = () => {
                hideLoading();
                pokemonDetails.style.display = "flex";
            };
            pokemonImg.onerror = () => {
                hideLoading();
                pokemonDetails.style.display = "flex";
            };
  
            pokemonTypes.innerHTML = "";
            pokemonData.types.forEach((type) => {
                const span = document.createElement("span");
                span.className = "type";
                span.textContent = type.type.name.toUpperCase();
                pokemonTypes.appendChild(span);
            });
  
            pokemonStats.innerHTML = "";
            pokemonData.stats.forEach((stat) => {
                const statDiv = document.createElement("div");
                statDiv.className = "stat";
  
                const statName = document.createElement("span");
                statName.textContent = stat.stat.name;
  
                const statValue = document.createElement("span");
                statValue.textContent = stat.base_stat;
  
                statDiv.appendChild(statName);
                statDiv.appendChild(statValue);
                pokemonStats.appendChild(statDiv);
            });
  
            const flavor = speciesData.flavor_text_entries.find(
                (entry) => entry.language.name == "en"
            );

            flavorText.textContent = cleanFlavorText(flavor.flavor_text);
  
        }     
        catch (err) {
            hideLoading();
            showError();
        }
    }
  
    loadPokemonList();
});
  